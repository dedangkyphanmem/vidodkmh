;#DropDB vidoWinSolutionEasyTest

#Application vidoWinSolutionWin
#Application vidoWinSolutionWeb

*Action Navigation(User)

*ProcessRecord
 Full Name = ''

#IfDef vidoWinSolutionWeb
*Action Edit
#EndIf
 
*FillForm
 Full Name = Test User
 
*Action Cancel

#IfDef vidoWinSolutionWin
*HandleDialog
 Respond = Yes
#EndIf

*CheckFieldValues
 Full Name = ''
