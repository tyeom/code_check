   M S B A M L   `   `   `  ����B  >WpfApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp2WpfApp2  5   	       WpfApp2.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp2     5   	   -   5        $��xMainWindow��5   	     � Title$  
MainWindow��5	   	   $	��450��6   $	��800��6)    � 	Resources 5
      (   "WpfApp2.CollectionToCountConverter   CollectionToCountConverter��%   &	          5   
   6+   5      .��5      � .+�5   
   �� -   5         ��Name$ xMenuTabControl��5      $��
0, 0, 0, 0q�5      ��5      �� *
MenuList+��5      �� 5       �� 
TargetType" ��6   .�5      ��  �� Property � �� �� Value 5      �� 5       �� 
TargetType" ��6.   .R�5   "   � $��70��6'   $	��150��5   '   $��0, 0, -1, -3q�5   '    � RowDefinitions 5   &   �� 5   *   $
��Auto �68   �� 5   *   $��* �68   .+�5   &   ��  ��Name$ xBorder1��6-   $��0��5    5   $��6��5!   5   ���	#FFD7DEE45"   5   �� 5#   &   $ xBorder2��6-   $��1��5$   5   ���	#FFD7DEE4$��
1, 2, 1, 1q�5&   5   ���	#FFEBF0F55'   5   �� 5(   &   	 ��Name$	 
xHeaderTxt��60   $��1��5)   8   $
��Wrap=�5+   8   
 �� TextAlignment$
 Center=�5,   8   ���	#FF7E8AA2$��Center=�5.   8   $��Center=�5/   8   ��5*   8   �� *
MenuName+5/   8   50   #    �� Triggers 51   "   P� 52   &    P� Property �@��
IsSelected P� Value . . �54   *   ��  �� 
TargetName$ xBorder1��61    � �� �	#FFEA5F8556   9   �� 57   *   $ xBorder2��61    � �� �	#FFFEF6F659   9   �� 5:   *   $ xBorder2��61    � ��$ 
1, 2, 1, 0q�5<   9   �� 5=   *   $ 
xHeaderTxt��61    � �� �	#FFEA5F855?   9   5@   '   5B      5D      5E       �� ContentTemplate 5G      �� 5H      .R�5I      � .+�5J      �� ��6!   �� *Desc+��5K   !   ��  �� ElementName$ xMainWindow�� �� Path$" DataContext.ShowDescMsgPopup���5L   !   �� *Desc+5M      5N      5P      (� 5S   
   .��5T         WpfApp2.LoadingControl  ��5U      �� 5V      "  6   .�5W      ��  � y�$ 	Collapsed=�67    �� Triggers 5X      �� 5Y       �� Binding 6*   �� $ xMenuTabControl��$ ItemsSource� �� 	Converter# [   �� Value 05\   *   .a�5]   "   ��  � y�$ Visible=�6?   5^      5`      5b      5c         WpfApp2.MessagePopUpBox  5g   
   -   5          Name$ 
xMsgBox_OK��5g          
HAlignment$ Center=�5h          
VAlignment$ Center=�5i       $	��550��5k          IsBackgroundDisable .    MessagePopUpBoxType$ Confirm=�5m          IsOpen 5j       �� *ShowMessagePopup+ �� Mode$ TwoWay=�.��5n      �� -   5        $	 
xTxtMsgBox��5n      $
 Center=�5p      $	��550��5q      $��25(�5r      $��	ExtraBold"�5s      $��
0, 0, 0, 0q�5t      ��5o      �� *SelectedMenuDesc+5t      5u      5w      5x      