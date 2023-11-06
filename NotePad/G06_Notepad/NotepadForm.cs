using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace G06_Notepad
{
	public partial class NotepadForm : Form
	{
		private string _filePath;
		private bool _isModified;

		public string FilePath
		{
			get { return _filePath; }
			set
			{
				_filePath = value;
				if (value == null)
				{
					this.Text = "Notepad - Untitled.txt";
				}
				else
				{
					this.Text = $"Notepad - {Path.GetFileName(value)}";
				}
			}
		}

		public NotepadForm()
		{
			InitializeComponent();
		}

		#region Event Handlers

		private void newToolStripMenuItem_Click(object sender, EventArgs e) => NewFile();

		private void openToolStripMenuItem_Click(object sender, EventArgs e) => OpenFile();

		private void saveToolStripMenuItem_Click(object sender, EventArgs e) => SaveFile();

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) => SaveFile(true);

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Close();

		private void NotepadForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!ConfirmSave())
				e.Cancel = true;
		}

		private void txtContent_TextChanged(object sender, EventArgs e)
		{
			if (!_isModified)
				_isModified = true;
		}

		#endregion

		private bool NewFile()
		{
			if (!ConfirmSave())
				return false;

			txtContent.Clear();
			FilePath = null;
			_isModified = false;
			return true;
		}

		private bool OpenFile()
		{
			if (!ConfirmSave())
				return false;

			if (dlgOpen.ShowDialog() == DialogResult.OK)
			{
				try
				{
					txtContent.Text = File.ReadAllText(dlgOpen.FileName);
					FilePath = dlgOpen.FileName;
					_isModified = false;
					return true;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			return false;
		}

		private bool SaveFile(bool isSaveAs = false)
		{
			if (FilePath == null || isSaveAs)
			{
				dlgSave.FileName = Path.GetFileName(FilePath);
				if (dlgSave.ShowDialog() == DialogResult.OK)
				{
					FilePath = dlgSave.FileName;
				}
				else
				{
					return false;
				}
			}

			try
			{
				File.WriteAllText(FilePath, txtContent.Text);
				_isModified = false;
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}

		private bool ConfirmSave()
		{
			if (!_isModified)
			{
				return true;
			}

			DialogResult result = MessageBox.Show(
				"Do you want to save changes?",
				"Confirmation",
				MessageBoxButtons.YesNoCancel,
				MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button3
			);

			switch (result)
			{
				case DialogResult.Yes:
					return SaveFile();
				case DialogResult.No:
					return true;
				default:
					return false;
			}
		}
	}
}
