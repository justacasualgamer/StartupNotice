#pragma warning disable IDE1006
using System.Diagnostics;
using System.IO;
using System.Windows.Media;

namespace StartupNotice
{
    public partial class Form1 : Form
    {
        // UPDATE VERSION
        private readonly string __version__ = "v1.0.1";
        // PLEASE UPDATE VERSION UP HERE
        private bool isDogging = false;
        private readonly string appdataDirectory = Environment.ExpandEnvironmentVariables("%userprofile%\\AppData\\Local\\StartupNotice");
        public Form1()
        {
            try
            {
                if (!Directory.Exists(appdataDirectory)) Directory.CreateDirectory(appdataDirectory);
                if (!File.Exists(Path.Combine(appdataDirectory, "options.ini")))
                {
                    File.CreateText(Path.Combine(appdataDirectory, "options.ini")).Dispose();
                }
                string[] readlines = File.ReadAllLines(Path.Combine(appdataDirectory, "options.ini"));
                if (!readlines.Any(item => item.Trim().StartsWith("[StartupOptions] Startup=")))
                {
                    DialogResult a = MessageBox.Show("Do you want to add StartupNotice to your startup files? (One time question)", "StartupNotice", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (a == DialogResult.Yes)
                    {
                        File.Copy(Environment.ProcessPath!, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "StartupNotice.exe"), overwrite: true);
                        File.AppendAllText(Path.Combine(appdataDirectory, "options.ini"), "[StartupOptions] Startup=true\n");
                    }
                    else
                    {
                        File.AppendAllText(Path.Combine(appdataDirectory, "options.ini"), "[StartupOptions] Startup=false\n");
                    }
                    File.AppendAllText(Path.Combine(appdataDirectory, "options.ini"), $"[VersionCheck] Version={__version__}\n");
                }
                else
                {
                    if (readlines.Any(item => item.Trim() == "[StartupOptions] Startup=true"))
                    {
                        List<string> writelines = [.. readlines];
                        bool foundversioncheck = false;
                        for (int i = 0; i < writelines.Count; i++)
                        {
                            if (writelines[i].Trim().StartsWith($"[VersionCheck] Version="))
                            {
                                writelines[i] = $"[VersionCheck] Version={__version__}";
                                foundversioncheck = true;
                                MessageBox.Show($"Successfully updated from version {readlines[i].Trim()[23..]} to version {__version__}", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        if (!foundversioncheck)
                        {
                            writelines.Add($"[VersionCheck] Version={__version__}");
                            MessageBox.Show($"Successfully updated to version {__version__}", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        File.WriteAllLines(Path.Combine(appdataDirectory, "options.ini"), writelines);
                        File.Copy(Environment.ProcessPath!, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "StartupNotice.exe"), overwrite: true);
                    }
                }
            }
            catch
            {
                MessageBox.Show($"It seems that the program could not access the file {Path.Combine(appdataDirectory, "options.ini")}. Please check your permissions and/or try again.");
            }
            finally
            {
                InitializeComponent();
                label2.Text = $"{__version__} by";
            }
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
                    pictureBox1.Left = 400;
                    isDogging = true;
                    using Stream stream = Properties.Resources.Dogsong;
                    string tempFilePath = Path.Combine(Path.GetTempPath(), $"dogsong{new Random().Next(2147483647)}.mp3");
                    using (FileStream fileStream = File.Create(tempFilePath))
                    {
                        stream.CopyTo(fileStream);
                    }
                    MediaPlayer mediaPlayer = new();
                    mediaPlayer.Open(new(tempFilePath));
                    mediaPlayer.Play();
                    Thread.Sleep(8000);
                    try
                    {
                        File.Delete(tempFilePath);
                    }
                    catch
                    {
                        ;
                    }
                    isDogging = false;
                }
            });
            Thread thread2 = new(() =>
            {
                pictureBox1.Visible = true;
                while (pictureBox1.Left > -60)
                {
                    pictureBox1.Left--;
                    Thread.Sleep(8);
                }
                pictureBox1.Visible = false;
            });
            thread.Start();
            thread2.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Thread thread = new(() =>
            {
                using Stream stream = Properties.Resources.DogBark;
                string tempFilePath = Path.Combine(Path.GetTempPath(), $"bark{new Random().Next(2147483647)}.mp3");
                using (FileStream fileStream = File.Create(tempFilePath))
                {
                    stream.CopyTo(fileStream);
                }
                MediaPlayer mediaPlayer = new();
                mediaPlayer.Open(new(tempFilePath));
                mediaPlayer.Play();
                Thread.Sleep(1000);
                try
                {
                    File.Delete(tempFilePath);
                }
                catch
                {
                    ;
                }
            });
            thread.Start();
        }
    }
}
