// -----------------------------------------------------------------------
//    tUnlocker - by atom0s (c) 2013 [atom0s@live.com]
//
//    tUnlocker is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    tUnlocker is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with tUnlocker.  If not, see <http://www.gnu.org/licenses/>.
// -----------------------------------------------------------------------

namespace tUnlocker
{
    using Extensions;
    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    public partial class frmMain : Form
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public frmMain()
        {
            InitializeComponent();
            this.lblVersion.Text = "Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Launch button click callback.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLaunchTerraria_Click(object sender, EventArgs e)
        {
            this.DoWork(true);
        }

        /// <summary>
        /// Patch button click callback.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPatchTerraria_Click(object sender, EventArgs e)
        {
            this.DoWork(false);
        }

        /// <summary>
        /// Launches or patches Terraria.
        /// </summary>
        /// <param name="launch"></param>
        private void DoWork(bool launch)
        {
            // Delete the existing log (if any..)
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tUnlocker.log")))
                File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tUnlocker.log"));

            this.lstLog.Items.Clear();
            this.AddToLog(string.Format("Starting tUnlocker [Mode: {0}]", launch.ToString()));
            this.AddToLog("Locating Terraria.exe...");

            // Attempt to locate Terrria..
            if (File.Exists("Terraria.exe"))
            {
                // Store this path for Terraria..
                Program.TerrariaPath = AppDomain.CurrentDomain.BaseDirectory;
                this.AddToLog("--> Done!");
            }
            else
            {
                // Attempt to locate Terraria in the registry..
                var path = Program.GetRegistryValue<string>("HKEY_LOCAL_MACHINE\\SOFTWARE\\Re-Logic\\Terraria", "Install_Path");
                if (path != null && File.Exists(Path.Combine(path, "Terraria.exe")))
                {
                    // Store this path for Terraria..
                    Program.TerrariaPath = path;
                    this.AddToLog("--> Done!");
                }
            }

            // Ask user where Terraria is if not found..
            if (string.IsNullOrEmpty(Program.TerrariaPath))
            {
                using (var ofd = new OpenFileDialog())
                {
                    ofd.AddExtension = true;
                    ofd.CheckFileExists = true;
                    ofd.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    ofd.Filter = @"Terraria Executable File (Terraria.exe)|Terraria.exe|All Files (*.*)|*.*";
                    ofd.FilterIndex = 0;
                    ofd.RestoreDirectory = true;
                    ofd.Title = @"Select your Terraria.exe file..";

                    if (ofd.ShowDialog() != DialogResult.OK)
                        return;
                    Program.TerrariaPath = Path.GetDirectoryName(ofd.FileName);

                    // Ensure we have a folder to use..
                    if (string.IsNullOrEmpty(Program.TerrariaPath))
                        return;
                    this.AddToLog("--> Done!");
                }
            }

            // Ensure the path is set..
            if (string.IsNullOrEmpty(Program.TerrariaPath))
            {
                this.AddToLog("--> Failed!");
                return;
            }

            try
            {
                // Set the working folder..
                Directory.SetCurrentDirectory(Program.TerrariaPath);

                /**
                 * Phase #1
                 * 
                 * Load Terraria Into Memory
                 */

                // Load Terraria into memory..
                this.AddToLog("Loading Terraria into memory..");
                var asm = AssemblyDefinition.ReadAssembly("Terraria.exe");
                this.AddToLog("--> Done!");

                /**
                 * Phase #2
                 * 
                 * Locate Functions To Patch
                 */

                // Locate Terraria.Main.Draw..
                this.AddToLog("Locating Terraria.Main.Draw..");
                var mainDraw = asm.GetMethod("Main", "Draw");
                if (mainDraw == null)
                {
                    this.AddToLog("--> Failed!");
                    return;
                }
                this.AddToLog("--> Done!");

                // Locate Terraria.Player.PlayerFrame..
                this.AddToLog("Locating Terraria.Player.PlayerFrame..");
                var playerFrame = asm.GetMethod("Player", "PlayerFrame");
                if (playerFrame == null)
                {
                    this.AddToLog("--> Failed!");
                    return;
                }
                this.AddToLog("--> Done!");

                // Locate Terraria.Player.UpdatePlayer..
                this.AddToLog("Locating Terraria.Player.UpdatePlayer..");
                var playerUpdate = asm.GetMethod("Player", "UpdatePlayer");
                if (playerUpdate == null)
                {
                    this.AddToLog("--> Failed!");
                    return;
                }
                this.AddToLog("--> Done!");

                // Locate Terraria.Player.ItemCheck..
                this.AddToLog("Locating Terraria.Player.ItemCheck..");
                var playerItemCheck = asm.GetMethod("Player", "ItemCheck");
                if (playerItemCheck == null)
                {
                    this.AddToLog("--> Failed!");
                    return;
                }
                this.AddToLog("--> Done!");

                // Locate Terraria.Steam.Init and Terraria.Steam.Kill..
                this.AddToLog("Locating Steam.Init & Steam.Kill..");
                var steamInit = asm.GetMethod("Steam", "Init");
                var steamKill = asm.GetMethod("Steam", "Kill");
                if (steamInit == null || steamKill == null)
                {
                    this.AddToLog("--> Failed!");
                    return;
                }
                this.AddToLog("--> Done!");

                /**
                 * Phase #3
                 * 
                 * Patch Functions
                 */

                // Attempt to patch Terraria.Main.Draw
                this.AddToLog("Patching Terraria.Main.Draw..");
                var drawPattern1 = new[] { OpCodes.Ldsfld, OpCodes.Ldloc_S, OpCodes.Bne_Un, OpCodes.Ldc_I4_0, OpCodes.Stloc_S, OpCodes.Ldloc_S, OpCodes.Ldfld, OpCodes.Ldc_I4_3, OpCodes.Beq_S }; // { OpCodes.Ldsfld, OpCodes.Ldloc_S, OpCodes.Bne_Un, OpCodes.Ldc_I4_0, OpCodes.Stloc_S, OpCodes.Ldsfld, OpCodes.Ldloc_S, OpCodes.Ldelem_Ref, OpCodes.Ldfld, OpCodes.Ldc_I4_3, OpCodes.Beq_S };
                var drawPattern2 = new[] { OpCodes.Ldloc_S, OpCodes.Ldfld, OpCodes.Ldc_I4_S, OpCodes.Bne_Un_S, OpCodes.Ldloc_S, OpCodes.Ldfld, OpCodes.Ldc_I4_S, OpCodes.Bne_Un_S }; //{ OpCodes.Ldsfld, OpCodes.Ldloc_S, OpCodes.Ldelem_Ref, OpCodes.Ldfld, OpCodes.Ldc_I4_S, OpCodes.Bne_Un_S };
                var drawOffset1 = mainDraw.ScanForPattern(0, drawPattern1);
                var drawOffset2 = mainDraw.ScanForPattern(drawOffset1, drawPattern2);
                if (drawOffset1 == -1 || drawOffset2 == -1)
                {
                    this.AddToLog("--> Failed!");
                    return;
                }
                mainDraw.RemoveRange(drawOffset1, drawOffset2 - drawOffset1);
                this.AddToLog("--> Done!");

                // Attempt to patch Terraria.Player.Frame
                this.AddToLog("Patching Terraria.Player.Frame #1..");
                var playerFramePattern1 = new[] { OpCodes.Ldsfld, OpCodes.Ldarg_0, OpCodes.Ldfld, OpCodes.Bne_Un };
                var playerFramePattern2 = new[] { OpCodes.Ldarg_0, OpCodes.Ldfld, OpCodes.Ldc_I4_S, OpCodes.Bne_Un_S };
                var playerFrameOffset1 = playerFrame.ScanForPattern(0, playerFramePattern1);
                var playerFrameOffset2 = playerFrame.ScanForPattern(0, playerFramePattern2);
                if (playerFrameOffset1 == -1 || playerFrameOffset2 == -1)
                {
                    this.AddToLog("--> Failed!");
                    return;
                }
                playerFrame.RemoveRange(playerFrameOffset1, playerFrameOffset2 - playerFrameOffset1);
                this.AddToLog("--> Done!");
                
                this.AddToLog("Patching Terraria.Player.Update #1..");
                //if (this.wingsLogic == 3 && Main.myPlayer == this.whoAmi)
                //    this.accRunSpeed = 0.0f;
                var playerUpdatePattern1 = new[] { OpCodes.Ldarg_0, OpCodes.Ldfld, OpCodes.Ldc_I4_3, OpCodes.Bne_Un_S, OpCodes.Ldsfld, OpCodes.Ldarg_0, OpCodes.Ldfld, OpCodes.Bne_Un_S, OpCodes.Ldarg_0, OpCodes.Ldc_R4, OpCodes.Stfld };
                var playerUpdateOffset1 = playerUpdate.ScanForPattern(0, playerUpdatePattern1);
                if (playerUpdateOffset1 == -1)
                {
                    this.AddToLog("--> Failed!");
                    return;
                }
                playerUpdate.ReplaceNops(playerUpdateOffset1, playerUpdatePattern1.Length);
                this.AddToLog("--> Done!");

                // Attempt to patch Terraria.Player.Update #2..
                this.AddToLog("Patching Terraria.Player.Update #2..");
                // if (Main.myPlayer == this.whoAmi && (this.wings == 3 || this.wings == 16 || this.wings == 17 || this.wings == 18 || this.wings == 19)) 
                // {
                //     num6 = 0f; 
                //     num3 *= 0.2f; 
                //     num4 = 0.2f;
                // }
                var playerUpdatePattern2 = new[] { OpCodes.Ldsfld, OpCodes.Ldarg_0, OpCodes.Ldfld, OpCodes.Bne_Un_S, OpCodes.Ldarg_0, OpCodes.Ldfld, OpCodes.Ldc_I4_3, OpCodes.Beq_S };
                var playerUpdateOffset2 = playerUpdate.ScanForPattern(0, playerUpdatePattern2);
                if (playerUpdateOffset2 == -1)
                {
                    this.AddToLog("--> Failed!");
                    return;
                }
                playerUpdate.ReplaceNops(playerUpdateOffset2 + 24, 15);
                this.AddToLog("--> Done!");
                
                // Attempt to patch Terraria.Player.Update #3..
                this.AddToLog("Patching Terraria.Player.Update #3..");
                // if (Main.myPlayer == this.whoAmi && (this.wings == 3 || this.wings == 16 || this.wings == 17 || this.wings == 18 || this.wings == 19))
                // {
                //     wingTime = 0; 
                //     jump = 0;
                // }
                var playerUpdatePattern3 = new[] { OpCodes.Ldsfld, OpCodes.Ldarg_0, OpCodes.Ldfld, OpCodes.Bne_Un_S, OpCodes.Ldarg_0, OpCodes.Ldfld, OpCodes.Ldc_I4_3, OpCodes.Beq_S, OpCodes.Ldarg_0, OpCodes.Ldfld, OpCodes.Ldc_I4_S, OpCodes.Beq_S, OpCodes.Ldarg_0, OpCodes.Ldfld, OpCodes.Ldc_I4_S, OpCodes.Beq_S, OpCodes.Ldarg_0, OpCodes.Ldfld, OpCodes.Ldc_I4_S, OpCodes.Beq_S, OpCodes.Ldarg_0, OpCodes.Ldfld, OpCodes.Ldc_I4_S, OpCodes.Bne_Un_S, OpCodes.Ldarg_0, OpCodes.Ldc_R4, OpCodes.Stfld, OpCodes.Ldarg_0, OpCodes.Ldc_I4_0, OpCodes.Stfld };
                var playerUpdateOffset3 = playerUpdate.ScanForPattern(0, playerUpdatePattern3);
                if (playerUpdateOffset3 == -1)
                {
                    this.AddToLog("--> Failed!");
                    return;
                }
                playerUpdate.ReplaceNops(playerUpdateOffset3 + 24, 6);
                this.AddToLog("--> Done!");

                // Attempt to patch Terraria.Player.ItemCheck #1..
                this.AddToLog("Patching Terraria.Player.ItemCheck #1..");
                var playerItemCheckPattern1 = new[] { OpCodes.Ldarg_0, OpCodes.Ldfld, OpCodes.Ldsfld, OpCodes.Bne_Un, OpCodes.Ldarg_0, OpCodes.Ldc_I4_S, OpCodes.Ldc_I4, OpCodes.Ldc_I4_1 };
                var playerItemCheckOffset1 = playerItemCheck.ScanForPattern(0, playerItemCheckPattern1);
                if (playerItemCheckOffset1 == -1)
                {
                    this.AddToLog("--> Failed!");
                    return;
                }
                playerItemCheck.ReplaceNops(playerItemCheckOffset1, 59);
                this.AddToLog("--> Done!");
                
                // Attempt to patch Terraria.Steam #1
                this.AddToLog("Patching Steam.Init & Steam.Kill..");
                steamInit.Body.Instructions[0].OpCode = OpCodes.Ldc_I4_1;
                steamInit.Body.Instructions[0].Operand = null;
                steamKill.InsertStart(Instruction.Create(OpCodes.Ret));
                this.AddToLog("--> Done!");

                this.AddToLog("Patching complete!");

                using (var mStream = new MemoryStream())
                {
                    // Write the new assembly to our memory stream..
                    asm.Write(mStream);

                    File.WriteAllBytes("derp.exe", mStream.GetBuffer());

                    // If launching exit now..
                    if (launch)
                    {
                        // Load the new Terraria assembly into memory..
                        Program.TerrariaAssembly = Assembly.Load(mStream.GetBuffer());
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }

                    this.AddToLog("Preparing to patch Terraria..");

                    // Create file backup for patching..
                    this.AddToLog("Creating Terraria.exe backup..");
                    if (File.Exists("Terraria.exe.bak"))
                        File.Delete("Terraria.exe.bak");
                    File.Copy("Terraria.exe", "Terraria.exe.bak");
                    this.AddToLog("--> Done!");

                    // Create the new Terraria.exe
                    this.AddToLog("Creating new Terraria.exe..");
                    using (var fStream = new FileStream("Terraria.exe", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        var mem = mStream.GetBuffer();
                        fStream.Write(mem, 0, mem.Length);
                    }
                    this.AddToLog("--> Done!");
                    this.AddToLog("Patching complete; you can now play!");
                }
            }
            catch (Exception ex)
            {
                this.AddToLog("Exception occurred!");
                this.AddToLog(ex.ToString());
                this.AddToLog("Invalid Terraria.exe or already patched!");

                if (launch)
                {
                    this.AddToLog("");
                    this.AddToLog("You cannot use tUnlocker to launch the game after it has been patched!");
                    this.AddToLog("Launch Terraria manually or via Steam!");
                }
            }
        }

        /// <summary>
        /// Adds a message to the log.
        /// </summary>
        /// <param name="message"></param>
        private void AddToLog(string message)
        {
            // Append message to the log file..
            using (var writer = File.AppendText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tUnlocker.log")))
                writer.WriteLine(message);
            this.lstLog.Items.Add(message);
            this.lstLog.SelectedIndex = this.lstLog.Items.Count - 1;
            this.lstLog.SelectedIndex = -1;
        }
    }
}
