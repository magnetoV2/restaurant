   M S B A M L   `   `   `  ����I  ERestaurant_Pos, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Restaurant_Pos.PagesRestaurant_Pos.Pages  5      /clr-namespace:Restaurant_PosRestaurant_Pos  5      $    Restaurant_Pos.Pages.LoginPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:Restaurant_Pos.Pages     5      /local1clr-namespace:Restaurant_Pos     5      H DMahApps.Metro, Version=1.6.5.1, Culture=neutral, PublicKeyToken=null;Controls,http://metro.mahapps.com/winfx/xaml/controls  5      U QMaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	      -   5          N�Name$  
Login_Page��5       N� Title$ 	LoginPage��5         ooredoo-Regular N� 
FontFamily# [  6    N� 
Background 5      �� 5   
    �� ImageSource$H B/Restaurant_Pos;component/Resources/Images/LoginBg/logobg 1080.png��6    j� Stretch$
 None=�6f    j� TileMode$
 None=�6u   .��5      � -   5         �Name$ 
login_grid��5   )   $��Stretch=�6   .+�5   
   �� -   5         ��Name$ Login_content��5   g   $��Center=�6   $��Center=�60   $	��400��6N   $	��400��6Z   $y�Visible=�6~   .+�5      �� 	 �� Orientation$	 
Horizontal=�6   $��Center=�62   $��-45,0,-10,0q�6O   $	��455��6d   $	��150��6p   .+�5      �� $L��F/Restaurant_Pos;component/Resources/Images/Icons/Zearo/login_icon1.png��6   $	��465��5      $
��Auto��6$   $��5,5,0,5q�62   $��Center=�5      
 J� RenderTransformOrigin$
 	0.577,0.5&�65   5*      �� 5,      -   5         ��Name$ Login_Email��52      
 � Tag$ User Name/Email��5-        Regular_TextBox#��[ 5.      $��20(�5/      $��1q�50      #��[  51      $��0,30,0,0q�52   '   $��5q�53      $
��Auto��6   6.   G� 55      -   5         G�Name$ Login_Password��5:      |�.   Regular_PasswordBox#��[ 57      $ Password��58      #��[  59      $��20(�5:      $��1q�5<      $��0,10,0,0q�5=      $��5q�6#   $
��Auto��6/   �� 5Q      -   5         ��Name$ Warehouse_DropDown_br��5Q      $y�Hidden=�5R       �� CornerRadius$ 2��5S      $��0,10,0,0q�5T      $��0.7,1,0.7,1q�5U        CartBlueBrush#��[ 63   ��5V      �� 5W       �� EndPoint$ 0.5,1&�6*    �� 
StartPoint$ 0.5,0&�6;   .��5X      �  � Color$ Black��6'    � Offset$ 0Q�65   � 5Y      $ 	#FFFBF2F2��6'   $ 1Q�69   5Z      ._�5\      �� -   5         ��Name$ Warehouse_DropDown��5\      $y�Hidden=�5]      $��25(�5^      $��5q�5`       �� SelectedIndex$ 0��5a      $��0q�5b      $��35��5c      ���	#FFF7F5F5��5_      Z� ��5g      �� 5h      .R�5i      �� $��20(�6G     OoredooArabic-Regular#��[ 6U   ��6(   �� *WarehouseName+6U   5j      5l      5m      �� 5o      -   5        $��btnLogin��5o   )   $��0 20 0 0q�6   $��0q�5p      $��1q�5q      $��35��6C   .��5r      �� $��Sign In��6    J� IsHitTestVisible .    ooredoo-light#��[ 6D   $��20(�6p   $��Normal"�6~   $	��105��6�   5s      5t      �� 5u   
   -	   5        $  ServerConfigration_content��5u   g   $��Center=�6   $��Center=�60   $	��400��6N   $	��400��6Z   $y�Hidden=�6�   .+�5w      ��   Regular_textblock#��[ 6   $��Server Configration��5x        Ooredoo-Heavy#��[ 5y      $��30(�5z      
���   �$��Center=�5}      $��56��5~      �� 5�      -
   5        $ Server_name��5�      $ Server Name��5�      #��[ 5�      $��20(�5�      $��1q�5�      #��[  5�      $��0,30,0,0q�5�   '   $��5q�5�      $
��Auto��6   6+   �� 5�      -   5        $ Server_port��5�      $ Server Port��5�      #��[ 5�      $��20(�5�      $��1q�5�      #��[  5�      $��0,20,0,0q�5�   '   $��5q�5�      $
��Auto��6   6+   �� 5�      -   5        $ ServerProcessingText��5�        Regular_TextBox_VaUp#��[ 5�      $��Center=�5�      $��Center=�6/   $�� ��5�      $��15(�6   $��0 20 0 0q�6(   �� 5�      -   5        $��0 10 0 0q�5�      $��0q�5�      $��1q�5�      $��35��6F   .��5�      �� $��Test Connection��6    .  #��[ 6L   $��20(�6x   $��Normal"�6�   5�      �� 5�      -   5        $��0 30 0 0q�5�      $��0q�5�      $��0q�5�      $��35��6I   ��5�      Z� ��6.   Z� .��5�      �� $	 
Horizontal=�6   .+�5�      '  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=null]]   Kind$  ArrowLeftDropCircleOutline=�6.   
���   �$��Center=�6d   $��0q�6   $��0q�6�   $��30��6�   $��30��6�   �� 5�      $
��Back��6     .  
���   �#��[ 6X   $��20(�6�   $��Normal"�6�   $��Center=�6�   $��5q�6�   5�      5�      5�      �� 5�   
   -   5        $ locDbConfigration_content��5�   g   $��Center=�6   $��Center=�60   $	��400��6N   $	��600��6Z   $y�Hidden=�6�   .+�5�      �� #��[ 6   $"��Local DataBase Configuration��5�      #��[ 5�      $��30(�5�      
���   �$��Center=�5�      $��56��5�      �� 5�      -   5        $ locDBServer_name��5�      $ Server Name��5�      #��[ 5�      $��20(�5�      $��1q�5�      #��[  5�      $��0,30,0,0q�5�   ,   $��5q�5�      $
��Auto��6   6+   �� 5�      -   5        $ locDbServer_port��5�      $ Server Port��5�      #��[ 5�      $��20(�5�      $��1q�5�      #��[  5�      $��0,20,0,0q�5�   ,   $��5q�5�      $
��Auto��6   �� 5�      -   5        $ dbName��5�      $ DataBase��5�      #��[ 5�      $��20(�5�      $��1q�5�      #��[  5�      $��0,20,0,0q�5�   "   $��5q�5�      $
��Auto��6   �� 5�      -   5        $ 	locDbUser��5�      $ 	User Name��5�      #��[ 5�      $��20(�5�      $��1q�5�      #��[  5�      $��0,20,0,0q�5�   %   $��5q�5�      $
��Auto��6   �� 5�      -   5        $ locDbPassword��5�      $ Password��5�      #��[ 5�      $��20(�5�      $��1q�5�      #��[  5�      $��0,20,0,0q�5�   )   $��5q�5�      $
��Auto��6   �� 5�      -   5        $ locServerProcessingText��5�      #��[ 5�      $��Center=�5�      $��Center=�6/   $�� ��5�      $��15(�6   $��0 20 0 0q�6(   �� 5�      -   5        $��locdbtestconnection��5�   "   $��0 10 0 0q�5�      $��0q�5�      $��1q�5�      $��35��6M   .��5�      �� $��Change��6    .  #��[ 6C   $��20(�6o   $��Normal"�6}   5�      �� 5�      -   5        $��0 30 0 0q�5�      $��0q�5�      $��0q�5�      $��35��6I   ��5�      Z� ��6.   Z� .��5�      �� $	 
Horizontal=�6   .+�5�        $  ArrowLeftDropCircleOutline=�6.   
���   �$��Center=�6d   $��0q�6   $��0q�6�   $��30��6�   $��30��6�   �� 5�      $
��Back��6     .  
���   �#��[ 6X   $��20(�6�   $��Normal"�6�   $��Center=�6�   $��5q�6�   5�      5�      5�      �� 5�   
   -   5        $ other_content��5�   g   $��Center=�6   $��Center=�60   $	��400��6N   $	��600��6Z   $y�Hidden=�6~   .+�5�      �� #��[ 6   $��Other Configuration��5�      #��[ 5�      $��30(�5�      
���   �$��Center=�5�      $��56��5      �� 5     -   5        $ cust_Display_Port��5     #��[ 5     $��20(�5     $ Customer Display Port��6   $��1q�5     #��[  5     $��0,30,0,0q�5  -   $��5q�5     $
��Auto��6   �� 5	     -   5        $ cust_display_BaudRate��5     $ Customer Display BaudRate��5
     #��[ 5     $��20(�5     $��1q�5     #��[  5     $��0,20,0,0q�5  1   $��5q�5     $
��Auto��6   �� 5     -   5        $ cash_Drawer_Port��5     #��[ 5     $��20(�5     $ Cash Drawer Port��6   $��1q�5     #��[  5     $��0,30,0,0q�5  ,   $��5q�5     $
��Auto��6   �� 5     -   5        $ cash_drawer_BaudRate��5      $" Cash Drawer Display BaudRate��5     #��[ 5     $��20(�5     $��1q�5     #��[  5     $��0,20,0,0q�5   0   $��5q�5!     $
��Auto��6   �� 5$     -   5        $��OtherssettingsSave��5%  "   $��0 10 0 0q�5$     $��0q�5%     $��1q�5&     $��35��6L   .��5'     �� $��Change��6    .  #��[ 6C   $��20(�6o   $��Normal"�6}   5(     �� 5*     -   5        $��0 30 0 0q�5*     $��0q�5+     $��0q�5,     $��35��6I   ��5-     Z� ��6.   Z� .��5.     �� $	 
Horizontal=�6   .+�5/       $  ArrowLeftDropCircleOutline=�6.   
���   �$��Center=�6d   $��0q�6   $��0q�6�   $��30��6�   $��30��6�   �� 50     $
��Back��6     .  
���   �#��[ 6X   $��20(�6�   $��Normal"�6�   $��Center=�6�   $��5q�6�   51     52     53     �� 55  
   $��Bottom=�6   $��20,10q�60   $y�Hidden=�6?   .+�56     �� $	 
Horizontal=�6   $
��Left=�62   $��Bottom=�6M   $��25��6h   .+�57     �� $��Device MAC : ��6   $	��0,2q�61   $��15(�6>   #��[  6L   $��25��6z   $	��Top=�6�   �� 58     -   5        $ 
macAddress��59     
������ $��0q�6
    �� 
IsReadOnly . $��15(�5;      �� TextWrapping$
 Wrap=�5<  
   ��5:  
   �� *Text+ �� Mode$ OneWay=�5<  
   5=     �� 5>     $	 
Horizontal=�6   $
��Left=�62   $��Bottom=�6M   $��25��6h   .+�5?     �� $��Date : ��6   $��0q�6+   $��15(�66   #��[  6D   $��25��6r   $	��Top=�6~   �� 5@     -    5        $ versiondate��5B      . $��
03/06/2020��5@  ,   #��[ 5A     $��Center=�5B  /   $	��Top=�6S   $��15(�6r   $��0,0,0,5q�6�   $��0q�6�   $��25��6�   5C     �� 5D     $	 
Horizontal=�6   $
��Left=�62   $��Bottom=�6M   $��25��6h   .+�5E     �� $��
Version : ��6   $��0q�6.   $��15(�69   #��[  6G   $��25��6u   $	��Top=�6�   �� 5F     -!   5        $ 
versiondet��5H      . $��V - 1.0��5F  ,   #��[ 5G     $��Center=�5H  .   $	��Top=�6R   $��15(�6q   $��0,0,0,5q�6   $��0q�6�   $��25��6�   5I     5J     �� 5K  
   -"   5        $ LoginProcessingText��5K     #��[ 5L     $��Bottom=�5M     $��Center=�5N     $��Center=�6/   $�� ��5O     $��15(�6   $��	0,0,0,180q�6(   � 5U  
   -#   5         �Name$ LoginProgressBar��5U     #��[ 5V     $��Bottom=�5W     $y�Hidden=�6'   $��	0,0,0,120q�6;   $��55��6O   $��55��6[   5Y     �� 5t  
   -$   5         ��Name$ SettingButton��5v     $��Right=�5t     $	��Top=�6.   ���#333333$��0q�5w     $��10q�5x     $��30��5{     $��30��5|     ��5y     Z� ��5z     Z� .��5~       $ Settings=�6&   $��30��67   $��30��6C    � ContextMenu 5�     �� 5�     -%   5         ��Name$ agentStatusContextMenu��5�  =   agentStatusContextMenu��$��15(�6]    	 Base_Regular#��[	 6k   .@�5�     m� -&   5         m�Name$ menulocdbsettings��5�     menuItemReadyStatus��$��Local DB Settings��5�     {�. $
��Left=�5�     $��0,5,0,5q�5�     $
��Auto��5�     $
��Auto��5�       � ToolTip$  Change Local DB Settings��5�     5�     m� 5�     -'   5        $ menuserverdbsettings��5�     menuItemAuxiliaryStatus��$��Server DB Settings��5�     {�. $
��Left=�5�     $��0,5,0,5q�5�     $
��Auto��5�     $
��Auto��5�     $  Change Server DB Settings��5�     5�     m� 5�     -(   5        $ 
menuothers��5�     menuItemAuxiliaryStatus��$��Other Settings��5�     {�. $
��Left=�5�     $��0,5,0,5q�5�     $
��Auto��5�     $
��Auto��5�     $  Change Other Settings��5�     5�     5�     5�     5�     5�     