#pragma warning disable IDE1006
using System.Diagnostics;

namespace StartupNotice
{
    public partial class Form1 : Form
    {
        private bool isDogging = false;
        private readonly string appdataDirectory = Environment.ExpandEnvironmentVariables("%userprofile%\\AppData\\Local\\StartupNotice");
        public Form1()
        {
            if (!Directory.Exists(appdataDirectory)) Directory.CreateDirectory(appdataDirectory);
            if (!File.Exists(Path.Combine(appdataDirectory, "options.ini"))) File.CreateText(Path.Combine(appdataDirectory, "options.ini"));
            try
            {
                string[] readlines = File.ReadAllLines(Path.Combine(appdataDirectory, "options.ini"));
                if (!readlines.Any(item => item.Trim() == "[StartupOptions] AskForStartupConfirmation=false"))
                {
                    DialogResult a = MessageBox.Show("Do you want to add StartupNotice to your startup files? (One time question)", "StartupNotice", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (a == DialogResult.Yes) File.Copy(Environment.ProcessPath!, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "StartupNotice.exe"));
                    File.AppendAllText(Path.Combine(appdataDirectory, "options.ini"), "[StartupOptions] AskForStartupConfirmation=false");
                }
            }
            catch
            {
                MessageBox.Show($"It seems that the program could not access the file {Path.Combine(appdataDirectory, "options.ini")}. Please check your permissions and/or try again.");
            }
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/justacasualgamer",
                UseShellExecute = true
            });
            linkLabel1.LinkVisited = true;
        }
        private void MenuScreen()
        {
            Text = "StartupNotice";
            Size = new(400, 500);
            label1.Visible = true;
            label2.Visible = true;
            linkLabel1.Visible = true;
            button1.Visible = true;
            button4.Visible = true;
            textBox1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
        }

        private void NotMenuScreen(string text, int W, int H)
        {
            Text = text;
            Size = new(W, H);
            label1.Visible = false;
            label2.Visible = false;
            linkLabel1.Visible = false;
            button1.Visible = false;
            button4.Visible = false;
            textBox1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = File.ReadAllText(Path.Combine(appdataDirectory, "todo.txt"));
            }
            catch (Exception)
            {
                try
                {
                    if (!Directory.Exists(appdataDirectory)) Directory.CreateDirectory(appdataDirectory);
                    if (!File.Exists(Path.Combine(appdataDirectory, "todo.txt"))) File.CreateText(Path.Combine(appdataDirectory, "todo.txt"));
                }
                catch (Exception ex)
                {
                    throw new Exception($"The program cannot create the directory or file at {appdataDirectory}. Please ensure this app has permission and try again. Details: {ex}");
                }
            }
            NotMenuScreen("TODO list", 300, 600);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            File.WriteAllText(Path.Combine(appdataDirectory, "todo.txt"), textBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MenuScreen();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Thread thread = new(() =>
            {
                if (isDogging)
                {
                    MessageBox.Show("Only one dogsong can play at a time!", "Dog", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    isDogging = true;
                    Console.Beep(262, 400);
                    Console.Beep(392, 400);
                    Console.Beep(196, 200);
                    Console.Beep(330, 200);
                    Console.Beep(294, 400);
                    Console.Beep(262, 400);
                    Console.Beep(294, 200);
                    Console.Beep(262, 400);
                    Console.Beep(196, 200);
                    Console.Beep(220, 200);
                    Console.Beep(262, 200);
                    Console.Beep(175, 400);
                    Console.Beep(220, 400);
                    Console.Beep(175, 200);
                    Console.Beep(262, 200);
                    Console.Beep(220, 400);
                    Console.Beep(247, 400);
                    Console.Beep(262, 200);
                    Console.Beep(294, 600);
                    Console.Beep(196, 400);
                    // second
                    Console.Beep(131, 400);
                    Console.Beep(392, 400);
                    Console.Beep(196, 200);
                    Console.Beep(330, 200);
                    Console.Beep(294, 400);
                    Console.Beep(262, 400);
                    Console.Beep(294, 200);
                    Console.Beep(262, 400);
                    Console.Beep(196, 200);
                    Console.Beep(220, 200);
                    Console.Beep(262, 200);
                    Console.Beep(349, 200);
                    Console.Beep(330, 200);
                    Console.Beep(294, 400);
                    Console.Beep(294, 200);
                    Console.Beep(262, 200);
                    Console.Beep(220, 400);
                    Console.Beep(220, 400);
                    Console.Beep(262, 400);
                    Console.Beep(262, 500);
                    isDogging = false;
                }
            });
            Thread thread2 = new(() =>
            {
                pictureBox1.Visible = true;
                while (isDogging)
                {
                    pictureBox1.Left--;
                    Thread.Sleep(12);
                }
                pictureBox1.Left = 400;
                pictureBox1.Visible = false;
            });
            thread.Start();
            thread2.Start();
        }
    }
}
