using System;
using System.Speech.Synthesis;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Linq;

namespace TalkingToDo
{
    public partial class Form1 : Form
    {
        private SpeechSynthesizer synth = new SpeechSynthesizer();

        public Form1()
        {
            InitializeComponent();

            synth.SetOutputToDefaultAudioDevice();

            // ListView 設定
            listViewTasks.View = View.Details;
            listViewTasks.CheckBoxes = true;
            /*listViewTasks.Columns.Add("完了", 50);
            listViewTasks.Columns.Add("タスク", 200);
            listViewTasks.Columns.Add("追加日時", 150);*/
            listViewTasks.FullRowSelect = true;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxTask.Text))
            {
                string taskText = textBoxTask.Text.Trim();
                string now = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                ListViewItem item = new ListViewItem(); // チェックボックス
                item.SubItems.Add(taskText);            // タスク
                item.SubItems.Add(now);                 // 日付

                listViewTasks.Items.Add(item);
                textBoxTask.Clear();
                textBoxTask.Focus();

                UpdateCompletedCount();
                UpdateStatusLabel();
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewTasks.SelectedItems)
            {
                listViewTasks.Items.Remove(item);
            }
            UpdateStatusLabel();
        }

        private void buttonSpeak_Click(object sender, EventArgs e)
        {
            if (listViewTasks.SelectedItems.Count > 0)
            {
                var item = listViewTasks.SelectedItems[0];
                string taskText = item.SubItems[1].Text;

                synth.SpeakAsync(taskText);
            }
        }

        private void listViewTasks_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            foreach (ListViewItem item in listViewTasks.Items)
            {
                if (item.Checked)
                {
                    item.ForeColor = System.Drawing.Color.Gray;
                }
                else
                {
                    item.ForeColor = System.Drawing.Color.Black;
                }
            }

            UpdateCompletedCount(); // ↓この関数でカウント表示も更新！
            UpdateStatusLabel();
        }

        private void UpdateCompletedCount()
        {
            int completed = 0;
            foreach (ListViewItem item in listViewTasks.Items)
            {
                if (item.Checked)
                    completed++;
            }

            labelStatus.Text = $"完了: {completed} 件 / 全体: {listViewTasks.Items.Count} 件";
        }

        private void UpdateStatusLabel()
        {
            int total = listViewTasks.Items.Count;
            int completed = listViewTasks.Items.Cast<ListViewItem>().Count(item => item.Checked);
            labelStatus.Text = $"完了: {completed} 件 / 全体: {total} 件";
        }
    }
}
