   M S B A M L   `   `   `  ����A  =Ping11, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:AutoPingAutoPing  5   	       AutoPing.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:AutoPing     5   	   \ XHardcodet.Wpf.TaskbarNotification, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null)tb http://www.hardcodet.net/taskbar  5   	   -   5          �Name$  window1��5   	    � Title$ AutoPing v1.2 (ifns 14)��5	   	   $	��500��6)   $	��900��66   ��5
      �� 5   
    �� EndPoint$ 0.5,1&�6    �� 
StartPoint$ 0.5,0&�6/   .��5      �  � Color$ Black��6    � Offset$ 0.016Q�6)   � 5      $ 	#FF5268F3��6   $ 1Q�6-   � 5      $ 	#FF5066EF��6   $ 0.993Q�6-   5      .��5      � $��0,50,0,0q�6   .+�5   
   �� .��5      � -   5        $��wrap1��5   ?   $��5q�6   $
��auto��6$   $
��auto��62   ��.  ?  9System.Windows.Controls.Primitives.SelectiveScrollingGrid$   SelectiveScrollingOrientation$ Vertical=�6r   $
��Auto=�6�   5      3  -Hardcodet.Wpf.TaskbarNotification.TaskbarIcon  5   
   -   5        $��mytray��5          MenuActivation$ LeftOrRightClick=�5#       � ContextMenu 5(   
   �� 5)      
�������.@�5*      m� -   5        $
��Show��5*      6)   m� 5-      -   5        $
��Exit��5-      6)   51      	   TrayToolTip	 54   
   �� 55      -   5        $��bordertooltip��56      
�������
 �� CornerRadius$
 4��58      
�������._�5;      �� -   5        $��tbtray��5;      $��Center=�5=      $��Center=�5>      5D      5I      � 5J   
   $
��Left=�6   $��39��6*   $��	0,-44,0,0q�66   $	��Top=�6I   $	��892��6a   .+�5K      �� -   5        $��опросить��5K      $
��Left=�6(   $��	10,10,0,0q�6C   $	��Top=�6V   $	��199��6n   6z   5L      5X      5\      