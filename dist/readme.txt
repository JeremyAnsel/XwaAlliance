
XwaAlliance is a new launcher for X-Wing Alliance.

# Installation

Make a backup of these files: Alliance.exe, Config.cfg, *.plt
Copy Alliance.exe, Alliance.exe.config, Alliance.jpg, AllianceTools.txt in your XWA directory.

To define the buttons shown in the launcher, edit the "AllianceTools.txt" file.
The format is:
name|path|closeWindow
or
name|path|arguments|closeWindow
name is the text of the button.
path is the filename of the tool.
arguments are the arguments of the tool.
closeWindow is true or false. true means that the launcher window is closed when the tool is run.

name can begin with a menu name. Example:
Menu 1\Sub Menu A\Name
To define a menu, path is empty. Example:
Menu 1|||true
Menu 1\Sub Menu A|||true
