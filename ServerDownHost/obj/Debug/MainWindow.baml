   M S B A M L   `   `   `  ����I  EServerDownHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:ServerDownHostServerDownHost  5   	       ServerDownHost.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:ServerDownHost     5   	   -   5          � Title$  ServerDown Host��5   	   $	��350��6!   $	��660��6.   $	��350��6:   $	��425��6J    � Icon$! Resources/netdrive_unav.png��6Y   .��5	      �  � ColumnDefinitions 5
   
   �� 5      �� 5      $��90 �6    � RowDefinitions 5   
   �� 5      �� 5      $��25 �6   .+�5   
   �� -   5         ��Name$ 
listBoxLog��5      $��10q�6&   �� 5   
   $��1��6   .+�5      �� -   5         ��Name$ buttonStart��5      $��Start��6*   $��
10,10,10,5q�6:   $��32��6N   �� 5      -   5        $ 
buttonStop��5      $
��Stop��6)   $��	10,5,10,5q�68   $��32��6K   5      �� 5   
   $��1��6   $��2��6!   .@�5      �� -   5        $��statusBarItemMessage��5      $��StatusBarItem��68   5      5      5      