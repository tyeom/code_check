   M S B A M L   `   `   `  ���� B  >WpfApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp2WpfApp2      WpfApp2.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:WpfApp2     -   $��xMainWindow��  � Title$  
MainWindow��$	��450��$	��800�� � 	Resources (   "WpfApp2.CollectionToCountConverter   CollectionToCountConverter��%   &	          .��� .+��� -    ��Name$ xMenuTabControl��$��
0, 0, 0, 0q����� *
MenuList+����  �� 
TargetType" ��.���  �� Property � �� �� Value ��  �� 
TargetType" ��.R�� $��70��$	��150��$��0, 0, -1, -3q� � RowDefinitions �� $
��Auto ��� $��* �.+���  ��Name$ xBorder1��$��0��$��6�����	#FFD7DEE4�� $ xBorder2��$��1�����	#FFD7DEE4$��
1, 2, 1, 1q����	#FFEBF0F5�� 	 ��Name$	 
xHeaderTxt��$��1��$
��Wrap=�
 �� TextAlignment$
 Center=����	#FF7E8AA2$��Center=�$��Center=����� *
MenuName+ �� Triggers P�  P� Property �@��
IsSelected P� Value . . ���  �� 
TargetName$ xBorder1�� � �� �	#FFEA5F85�� $ xBorder2�� � �� �	#FFFEF6F6�� $ xBorder2�� � ��$ 
1, 2, 1, 0q��� $ 
xHeaderTxt�� � �� �	#FFEA5F85 �� ContentTemplate �� .R�� .+��� ���� *Desc+���� *ShowDescMsgPopup+���� *Desc+(� .��   WpfApp2.LoadingControl  ���� "  .���  � y�$ 	Collapsed=� �� Triggers ��  �� Binding ��  �� ElementName$ xMenuTabControl�� �� Path$ ItemsSource� �� 	Converter# [   �� Value 0.a���  � y�$ Visible=�   WpfApp2.MessagePopUpBox  -     Name$ 
xMsgBox_OK��   
HAlignment$ Center=�   
VAlignment$ Center=�$	��550��   IsBackgroundDisable .    MessagePopUpBoxType$ Confirm=�   IsOpen �� *ShowMessagePopup+ �� Mode$ TwoWay=�.���� -   $	 
xTxtMsgBox��$
 Center=�$	��550��$��25(�$��	ExtraBold"�$��
0, 0, 0, 0q����� *SelectedMenuDesc+