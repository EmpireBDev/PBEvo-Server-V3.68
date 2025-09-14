using Executable.Utility;
using Plugin.Core.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Executable.Forms
{
    public partial class MainForm : Form, IMessageFilter
    {
        #region Moveable
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public const int WM_LBUTTONDOWN = 0x0201;
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        #endregion Moveable
        #region Corner
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthElipse, int nHeightElipse);
        #endregion Corner
        private readonly HashSet<Control> ControlsToMove = new HashSet<Control>();
        private readonly int ProcessId;
        private readonly DirectoryInfo ServerDir;
        private readonly Color BaseColor = Color.FromArgb(20, 30, 54);
        private readonly Color SelectedColor = Color.FromArgb(46, 51, 73);
        public MainForm(int ProcessId, DirectoryInfo ServerDir)
        {
            InitializeComponent();
            this.ProcessId = ProcessId;
            this.ServerDir = ServerDir;
            Application.AddMessageFilter(this);
            ControlsToMove.Add(this);
            ControlsToMove.Add(MonitorLogo);
            ControlsToMove.Add(MonitorName);
        }
        #region Attachments
        private void LoadCleanLabels()
        {
            AppTitle.Text = "Version ****";
            AppStatus.Text = "PLEASE WAIT...";
            MemoryVPB.Maximum = MemoryUtility.TotalMemory();
            RamPercent.Text = "--";
            LogFileSize.Text = "--";
        }
        public bool PreFilterMessage(ref Message Msg)
        {
            if (Msg.Msg == WM_LBUTTONDOWN && ControlsToMove.Contains(FromHandle(Msg.HWnd)))
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                return true;
            }
            return false;
        }
        private IEnumerable<Control> GetAllControls(Control CTRL)
        {
            Stack<Control> STC = new Stack<Control>();
            STC.Push(CTRL);
            while (STC.Any())
            {
                Control NextCTRL = STC.Pop();
                foreach (Control ChildCTRL in NextCTRL.Controls)
                {
                    STC.Push(ChildCTRL);
                }
                yield return NextCTRL;
            }
        }
        private void ValidateFont(string[] Files, PrivateFontCollection PFC)
        {
            foreach (string File in Files)
            {
                PFC.AddFontFile(File);
            }
        }
        private string FontName(FontFamily[] FamililyArray)
        {
            foreach (FontFamily Family in FamililyArray)
            {
                if (Family.Name == FontSet())
                {
                    return Family.Name;
                }
            }
            return "Consolas";
        }
        private string FontSet()
        {
            string Text = "";
            try
            {
                string Path = "Config/FontSet.ini";
                if (!File.Exists(Path))
                {
                    MessageBox.Show($"File Not Found! {Path}", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return Text;
                }
                string[] Lines = File.ReadAllLines(Path, Encoding.UTF8);
                foreach (string Line in Lines)
                {
                    if (!(Line.StartsWith(";") || Line.StartsWith("[")))
                    {
                        Text = Line;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return Text;
        }
        #endregion Attachments
        private void Monitor1_Load(object sender, EventArgs e)
        {
            Rectangle WorkingArea = Screen.GetWorkingArea(this);
            Location = new Point(WorkingArea.Right - Size.Width, WorkingArea.Bottom - Size.Height);
            FormLoaderPNL.BackColor = SelectedColor;
            MonitorPanelN.BackColor = SelectedColor;
            AdditionalPanelN.BackColor = SelectedColor;
            ConfigPanelN.BackColor = SelectedColor;
            LoadCleanLabels();
            using (PrivateFontCollection PFC = new PrivateFontCollection())
            {
                string[] Files = Directory.GetFiles($"Font/");
                if (Files.Length > 0)
                {
                    ValidateFont(Files, PFC);
                    string SelectedFont = FontName(PFC.Families);
                    foreach (Control CTRL in GetAllControls(this))
                    {
                        CTRL.Font = new Font(SelectedFont, CTRL.Font.Size, CTRL.Font.Style);
                    }
                }
                else
                {
                    MessageBox.Show("The Font was not found. try again!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    WindowUtility.KillProcessAndChildren(ProcessId);
                }
            }
            FormLoaderPNL.BringToFront();
            NavPNL.Width = FMonitorPanel.Width;
            NavPNL.Top = FMonitorPanel.Top;
            NavPNL.Left = FMonitorPanel.Left;
            MonitorBTN.BackColor = SelectedColor;
            AdditionalBTN.BackColor = BaseColor;
            ConfigBTN.BackColor = BaseColor;
            
            FormLoaderPNL.Controls.Clear();
            FormMonitor Monitor = new FormMonitor()
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true
            };
            FormLoaderPNL.Controls.Add(Monitor);
            Monitor.Show();
            
            RefresherT.Start();
        }
        private void RefreshBTN_Click(object sender, EventArgs e)
        {
            Refresh();
        }
        private void CloseBTN_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void MinimizeBTN_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        private void Monitor1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle BorderRectangle = ClientRectangle;
            BorderRectangle.Inflate(0, 0);
            ControlPaint.DrawBorder(e.Graphics, BorderRectangle, Color.FromArgb(255, 54, 54, 164), ButtonBorderStyle.Solid);
        }
        private void Monitor1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult Result = MessageBox.Show("Are you sure want to quit?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                WindowUtility.KillProcessAndChildren(ProcessId);
            }
            e.Cancel = (Result == DialogResult.No);
        }
        private void RefresherT_Tick(object sender, EventArgs e)
        {
            AppTitle.Text = $"Version {StringUtility.ServerVersionL}";
            AppStatus.ForeColor = (StringUtility.ServerStatusL.Equals("SERVER ONLINE") ? ColorUtil.Green : StringUtility.ServerStatusL.Equals("SERVER OFFLINE") ? ColorUtil.Red : ColorUtil.White);
            AppStatus.Text = StringUtility.ServerStatusL;
            MemoryVPB.Value = int.Parse(StringUtility.MemoryValueVPB);
            RamPercent.Text = StringUtility.MemoryUsageL;
            LogFileSize.Text = StringUtility.LogFileSize;
        }
        private void MonitorBTN_Click(object sender, EventArgs e)
        {
            NavPNL.Width = FMonitorPanel.Width;
            NavPNL.Top = FMonitorPanel.Top;
            NavPNL.Left = FMonitorPanel.Left;
            MonitorBTN.BackColor = SelectedColor;
            AdditionalBTN.BackColor = BaseColor;
            ConfigBTN.BackColor = BaseColor;

            FormLoaderPNL.Controls.Clear();
            FormMonitor Monitor = new FormMonitor()
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true
            };
            FormLoaderPNL.Controls.Add(Monitor);
            Monitor.Show();
        }
        private void AdditionalBTN_Click(object sender, EventArgs e)
        {
            NavPNL.Width = FAdditionalPanel.Width;
            NavPNL.Top = FAdditionalPanel.Top;
            NavPNL.Left = FAdditionalPanel.Left;
            AdditionalBTN.BackColor = SelectedColor;
            MonitorBTN.BackColor = BaseColor;
            ConfigBTN.BackColor = BaseColor;

            FormLoaderPNL.Controls.Clear();
            FormAdditional Additional = new FormAdditional()
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true
            };
            FormLoaderPNL.Controls.Add(Additional);
            Additional.Show();
        }
        private void ConfigBTN_Click(object sender, EventArgs e)
        {
            NavPNL.Width = FConfigPanel.Width;
            NavPNL.Top = FConfigPanel.Top;
            NavPNL.Left = FConfigPanel.Left;
            ConfigBTN.BackColor = SelectedColor;
            MonitorBTN.BackColor = BaseColor;
            AdditionalBTN.BackColor = BaseColor;

            FormLoaderPNL.Controls.Clear();
            FormConfig Config = new FormConfig(ServerDir)
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true
            };
            FormLoaderPNL.Controls.Add(Config);
            Config.Show();
        }
    }
}
