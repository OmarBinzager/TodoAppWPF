   M S B A M L   `   `   `  ����F  BToDoProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:ToDoProject.View.ComponentsToDoProject.View.Components  5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089/    )ToDoProject.View.Components.CustomTextBox  �� 
TargetType �� Property   secondIconColor �� Value  dialogBackground  
Montserrat �� 
TargetType �� CornerRadius ��Name    Hint  MontsRegular13 ��Name �� TextWrapping-   'ToDoProject.View.Components.AuthTextBox  NoErrorTemplate	 � RowDefinitions
 �� Padding � ColumnDefinitions F�Name   IconData �� Stretch  authIconColor   HintText  hintFontColor  MontsMedium16   	ErrorText  boolToVisibility �� 	Converter �� Triggers 	 AuthActionButton 
 primaryColor  secondaryColor ��Name J� Effect3  -System.Windows.Media.Effects.DropShadowEffect   
BlurRadius   	Direction   Opacity   ShadowDepth �� RelativeSource :� RoutedEvent �� 
TargetName �� TargetProperty g� Duration	 W� To2   ,ToDoProject.View.Components.AuthActionButton   Text�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      3local)clr-namespace:ToDoProject.View.Components  5      %�  '         7   7  7  '  J    7  7  7  7  7  7  7  &		 ]    7  '  &
    7 
 7  �� 5      "    6   .�5   
   ��  � ��8   6(   �� 5	   
    � ��8  6'   �� 5
   
    � ��8  6'   �� 5   
    � �� 5      �� 5      "   6"   .R�5      � .+�5      �� $��1��6!   #��z��6.   #��z��6X   $��1q�6�   $ 6��6�   ._�5      � .+�5   "   �� $
 hint��6,   #��z 6:   $��	12,10,0,0q�6X   #��z��6k   #���  6�   �� 5   "   $ 	typingBox��6*   $
 Wrap=�6=   
������ $��13(�6j   #��z��6x   #��z��6�   $��Regular"�6�   $��10q�6�   $��0q�6�   5      5      5      5      5      5      �� 5      "   6   .�5   
   ��  �@=�ErrorTemplate8   65   �� 5   
    � �� 5      �� 5      "  6"   .R�5       � 	 5!      �� 5"      �� 5#      $
��Auto �6,   .+�5%      �� 
������ ���#565454$ 8��5'   !   $��60��5(   !   $
 18 15q�5)   !   $��1q�5*   !   ._�5+      �  5,   "   �� 5-   &   $��30 �67   �� 5.   &   $��22 �67   �� 5/   &   .+�51   "   �� $��30��6)   $��30��64   $��Center=�6@   $��Center=�6]   ._�52   &   F� $
 icon��6+   #��z 69   $��Center=�6[   $��Center=�6x   $
 None=�6�   8�� 6�   53   #   �� 54   "   $ HintTextBlock��6,   #��z 6C   8�� $��Center=�6�   $��2��6�   8�� 6�   �� 55   "   $ TextBox��6X   $��2��6*   #��z��6:   
������ $��Center=�6�   $��0q�6�   $��16(�6�   8�� $��Medium"�6�   56      57      �� 58      $��0 10 0 0q�6$   $��1��66   #��z 6C   8�� 
���  ��y�6f   �� *ErrorTextVisibility+8  6�   59       5:      5<      5>      5?      �� 5A      "  ��6   .�5B   
   ��  � ��# � 
 6'   �� 5C   
    � ��# �  6'   �� 5D   
    � ��$ 15(�6%   �� 5E   
    � �� 5F      �� 5G      .R�5H      �� $ border��6   #��z��6-   $ 5��6W   $
 40 20q�5I   -   $
��Auto��5J   -   $
��Auto��5K   -   $
��Left=�5L   -    5M        5N      $ 5Q�6/   $ 0Q�6>   $ 0Q�6L   $ 0Q�6X   ._�5P      �� 
�������8��  $��Center=�6�   $��Center=�6�   ��6$   �� *	Content+ �� *TemplatedParent+6�   5Q       5R      :� 5S      $ Mouse.MouseEnter��6'   .[�5T      �� .��5U   "   �� $ border��6-   $ Effect.Opacity�6L   .��5V   &   W� $ 0:0:.3D�66   $ .4X�6H   5W   #   5X      5Y      :� 5Z      $ Mouse.MouseLeave��6'   .[�5[      �� .��5\   "   �� $ border��6-   $ Effect.Opacity�6L   .��5]   &   W� $ 0:0:.3D�66   $ 0X�6H   5^   #   5_      5`      5b      5d      5u      �� 5w      "   6   .�5x   
   ��  � �� 5y      �� 5z      "  6"   .R�5{      �� $ 
ButtonArea��5|      $ 5��5}      $
 40 20q�5~      $
��Auto��5      $
��Auto��5�      $
��Left=�5�      8��  ._�5�      �� #��z 6(   
�������8�� $��Center=�6�   $��Center=�6�   5�      5�      5�      5�      5�      