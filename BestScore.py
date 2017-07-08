import clr
import math
clr.AddReference('System.Windows.Forms')
from System.Windows.Forms import *

def fileOpen(sender, e):
    file = open(r'Scores.txt')
    m = [row.split() for row in file.read().split("\n") if row]
    sum = 0
    score = []
    for i in range(len(m)):
        score.append(int(m[i][3]))
    sum = 0
    for i in range(len(score)):
        sum += score [i]
    maxScore = max(score)
    frm = sender.Tag
    frm.textBox2.Text = str(maxScore)

def LoadExtension(frm):  
    newExtension = ToolStripMenuItem(Text = 'Best Score')
    newExtension.Tag = frm
    newExtension.Click += fileOpen
    frm.statisticsToolStripMenuItem.DropDownItems.Add(newExtension)



