import clr
import math
clr.AddReference('System.Windows.Forms')
from System.Windows.Forms import *

def fileOpen(sender, e):
    dialogResult = MessageBox.Show("Are you sure you want to delete the progress?", "Warning", MessageBoxButtons.YesNo)
    if(dialogResult == DialogResult.Yes):
        file = open('Scores.txt', 'w')
        file.truncate()
        file.close()
        frm = sender.Tag
        frm.textBox2.Text = ""
        frm.textBox3.Text = ""
        frm.textBox4.Text = ""
        frm.textBox5.Text = ""

def LoadExtension(frm):  
    newExtension = ToolStripMenuItem(Text = 'Delete All')
    newExtension.Tag = frm
    newExtension.Click += fileOpen
    frm.statisticsToolStripMenuItem.DropDownItems.Add(newExtension)

   


     
    



