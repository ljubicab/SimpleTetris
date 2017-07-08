import clr
import math
clr.AddReference('System.Windows.Forms')
from System.Windows.Forms import *

def fileOpen(sender, e):
    file = open(r'C:\Users\Ljubica\Desktop\ProjektniZadatak\Scores.txt')
    m = [row.split() for row in file.read().split("\n") if row]
    sum = 0
    score = []
    for i in range(len(m)):
        score.append(float(m[i][3]))
    sum = 0
    for i in range(len(score)):
        sum += score [i]
    minScore = min(score)
    frm = sender.Tag
    frm.textBox3.Text = str(minScore)

def LoadExtension(frm):  
    newExtension = ToolStripMenuItem(Text = 'Worst Score')
    newExtension.Tag = frm
    newExtension.Click += fileOpen
    frm.statisticsToolStripMenuItem.DropDownItems.Add(newExtension)




