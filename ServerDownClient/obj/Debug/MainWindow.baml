   M S B A M L   `   `   `  ����K  GServerDownClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:ServerDownClientServerDownClient  5   	   !    ServerDownClient.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:ServerDownClient     5   	   -   5          � Title$  ServerDown Client��5   	   $	��350��6#   $	��600��60   $	��250��6<   $	��350��6L    � Icon$! Resources/netdrive_unav.png��6[   .��5	      �  � RowDefinitions 5
   
   �� 5      $��20 �6   �� 5      �� 5      $��25 �6   .+�5   
   o� -   5         o�Name$
 menu��5      .@�5      m� -   5        $��menuItemOptions��5      $��Options��6.   .@�5      m� -   5        $��menuItemOptionsSave��5      $
��Save��66   m� 5      -   5        $��menuItemOptionsSaveAs��5      $��Save as��68   m� 5      -   5        $��menuItemOptiosMail��5      $
��Mail��65   5      5      � 5   
   $��0q�6   $��1��6    � ColumnDefinitions 5      �� 5      �� 5      $��88 �6#   .+�5      �� -   5         ��Name$ 
listBoxLog��5      $��10q�6*   �� 5      $��1��6   .+�5      �� -   5         ��Name$ buttonSaveLog��5      $��
10,10,10,5q�60    � Cursor$
 Hand��6D   .��5      �� $��Resources/save2.png��6   $��5q�69   5      �� 5       -	   5        $ buttonMailLog��5       $��	10,5,10,5q�60   $
 Hand��6C   .��5!      �� $��Resources/mail2.png��6   5"      5#      5$      �� 5%   
   $��0q�6   $��2��6   .@�5&      �� -
   5        $��statusBarItemMessage��5&      $��StatusBarItem��68   $��0q�6P   5'      5(      5)      