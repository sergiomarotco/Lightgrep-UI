using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Lightgrep_UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/sergiomarotco/Lightgrep-UI");
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://ru.icons8.com"); // открыть рекламную ссылку
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new())
            {
                openFileDialog.InitialDirectory = Environment.CurrentDirectory;
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new(fileStream))
                    {
                        string fileContent = reader.ReadToEnd();
                    }
                }
            }
            Search(filePath, "");
        }

        private void Search(string file_Path, string text_for_grep)
        {
            string text = "";
            List<string> Matches = new();
            if (!file_Path.Equals("")) // поиск по файлу
            {
                text = File.ReadAllText(file_Path);
            }
            else
            {
                if (!string.IsNullOrEmpty(text_for_grep)) // поиск по тексту
                {
                    text = text_for_grep;
                }
            }

            var m1 = Regex.Matches(text, textBox1.Text);
            foreach (Match match in m1.Cast<Match>())
            {
                Matches.Add(match.Value);
            }

            if (Matches.Count > 0)
            {
                DateTime dateTime = DateTime.Now;
                string result = Path.GetTempPath() + "tempgrep " + dateTime.Ticks+ ".txt";
                if (File.Exists(result))
                {
                    File.Delete(result);
                }

                File.WriteAllLines(result, Matches);
                Process txt = new();
                txt.StartInfo.FileName = "notepad.exe";
                txt.StartInfo.Arguments = result;
                txt.Start();
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Search("", Clipboard.GetText());
        }
        /// <summary>
        /// Поиск в тексте из файла полученного при помощи drag and drop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                {
                    if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    {
                        string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop);

                        if (fileList != null)
                        {
                            if (fileList.Length == 1)
                            {
                                Search(fileList[0], "");
                            }
                        }
                    }
                }
            }
        }
    }
}