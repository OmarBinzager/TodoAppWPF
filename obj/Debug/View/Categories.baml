   M S B A M L   `   `   `  ����F  BToDoProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:ToDoProject.ViewToDoProject.View  5      =#clr-namespace:ToDoProject.ViewModelToDoProject.ViewModel  5      !    ToDoProject.View.Categories   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:ToDoProject.View     5      2vm#clr-namespace:ToDoProject.ViewModel     5      D @ModernWpf, Version=0.9.6.0, Culture=neutral, PublicKeyToken=nullM IModernWpf.Controls, Version=0.9.6.0, Culture=neutral, PublicKeyToken=null0modern!http://schemas.modernwpf.com/2019   66     N� Title$  
Categories��5
         	pageStyle#���   6    � DataContext 5      /   )ToDoProject.ViewModel.CategoriesViewModel  5   
   .��5      � .+�5   
   ��   bordedContainer#��[ 6    �� Padding$ 20, 20,20,0q�6:   ._�5      �  � RowDefinitions 5      �� 5      $��50 �6$   �� 5      �� 5      .+�5      �� .+�5      ��   InterSemiBold24#��[ 6$   $
��Left=�6M   $��Task Categories��6h   $��0,0,0,2q�6   �� 5      $��50��6!   $
��Left=�6,   $��2��6G     orangeColor#��[ 6R    �� CornerRadius$ 1��6|   5      �� 5      $��1��6   .+�5      � $��0 0 0 15q�6   .+�5      �� .+�5      ��   InterSemiBold15#��[ 6(   $
��Left=�6Q   $��Task Categories��6l   $��0,0,0,2q�6�   �� 5      $��50��6%   $
��Left=�60   $��2��6K   #��[ 6V   $ 1��6�   5       �� 5!      $��1��6!     addTaskButton#��[ 61   $��Add Category��6   ��6X   �� *AddCategoryCommand+6   5"      �� 5#      
������ ��.  �� PanningMode$ VerticalOnly=�5$      $	��350��6A   $
��Auto��6Q   $��0q�5%      $��Disabled=�5&      $
��Auto=�6Q   ��5#      �� *
Categories+��5(      �� 5*   "    �� 
TargetType" ��6(   .�5+   &   ��  �� Property � �� �� Value 5,   *   �� 5-   .   	 �� 
TargetType"	 ��6>   .R�5.   2   �� 
 ��Name$
 Border��69   $ 8��5/   )   $��1q�50   )     secondIconColor#��[ 51   )   
������ $��5,3,5,3q�53   )   $ 10q�54   )   ._�55   6   �  � ColumnDefinitions 56   :   �� 57   >   $��* �6O   �� 58   >   $
��Auto �6O   .+�5<   :   �� $��Center=�5=   4     MontsMedium15#��[ 6O   ��5<   D   �� *Name+5=   O   �� 5@   :   $��1��6E    �� Orientation$ 
Horizontal=�6U   .+�5A   >   �� $
��Edit��6E     EditActionButton#��[ 5D   5   $��5,0,0,0q�5E   5   ��5B   5   �� *DataContext.EditCommand+ �� RelativeSource ��  �� AncestorType$ ListViewL���5C   5   �� 5E   5   �� 5F   >   $��Delete��6E    	 DeleteActionButton#��[	 5I   5   $��5,0,0,0q�5J   5   ��5G   5   �� *DataContext.DeleteCommand+ �� $ ListViewL���5H   5   �� 5J   5   5K   ;   5L   7   5M   3   5N   /   5P   '   �� 5Q   &    � ��$ Stretch=�6S   5R   #   5T      %  ModernWpf.Controls.ProgressRing  5U      $��60��5V      $��60��6!    
 primaryColor#��[
 5W      $��Center=�5X         IsActive 5U   *   �� *CategoryIsLoading+y�5Y      �� *CategoryIsLoading+  boolToVisibility �� 	Converter# [ �� 5Z      $��Center=�6    $��Center=�5[        MontsMedium16#��[ 64   #��[ 6[   ��6�   �� *NoCategoriesMessage+y�6�   �� *NoCategoriesMessage+   StringToImageSourceConverter# [ 5\      �� 5]      $��2��6   $��15q�6*   .+�5^      � $��0 0 0 15q�6   .+�5_      �� .+�5`      �� #��[ 6(   $
��Left=�6Q   $��Task Priorities��6l   $��0,0,0,2q�6�   �� 5a      $��50��6%   $
��Left=�60   $��2��6K   #��[ 6V   $ 1��6�   5b      �� 5c      $��1��6!   #��[ 61   $��Add Priority��6   ��6X   �� *AddPriorityCommand+6   5d      �� 5e      
������ $��0q�5f      ��. $ VerticalOnly=�5g      $	��350��6A   $
��Auto��6Q   $��Disabled=�5h      $
��Auto=�6Q   ��5e       �� *
Priorities+��5i      �� 5k      " ��6$   .�5l   "   ��  � �� 5m   &   �� 5n   *   "	 ��6:   .R�5o   .   �� $
 Border��65   $ 8��5p      $��1q�5q      #��[ 5r      
������ $��5,3,5,3q�5t      $ 10q�5u      ._�5v   2   �  5w   6   �� 5x   :   $	��.1* �6K   �� 5y   :   $��* �6K   �� 5z   :   $
��Auto �6K   .+�5~   6   A� $��13��6>   $��13��6I   $��5,0,0,0q�6U   $
��Left=�6}   ��6f   �� *Color+6}   �� 5   6   $��1��6@   $��Center=�5�      #��[ 67   ��5   P   �� *Name+5�   7   �� 5�   6   $��2��6A   $ 
Horizontal=�6Q   .+�5�   :   �� $
��Edit��6A   #��[ 5�      $��5,0,0,0q�5�      ��5�      �� *DataContext.EditCommand+ �� $ ListViewL���5�      �� 5�      �� 5�   :   $��Delete��6A   #��[	 5�      $��5,0,0,0q�5�      ��5�      �� *DataContext.DeleteCommand+ �� $ ListViewL���5�      �� 5�      5�   7   5�   3   5�   /   5�   +   5�   #   �� 5�   "    � ��$ Stretch=�6O   5�      5�        5�      $��60��5�      $��60��6)   #��[
 5�      $��Center=�5�       5�   *   �� *PriorityIsLoading+y�5�   !   �� *PriorityIsLoading+# [ �� 5�      $��Center=�6    $��Center=�5�      #��[ 6    #��[ 6G   ��6u   �� *NoPrioritiesMessage+y�6�   �� *NoPrioritiesMessage+# [ 5�      �� 5�      $��1��6   $��2��6,   $
��Auto��6=   $��1��6J   #��[ 6V   $��Stretch=�6�   5�      5�      5�      5�      