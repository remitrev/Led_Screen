   M S B A M L   `   `   `  ����E  ALed_Screen, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Led_Screen
Led_Screen  5   	       Led_Screen.MainWindowView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:Led_Screen     5   	     � Title$  
MainWindow��5   	   $	��400��6   $	��927��6)    � 
ResizeMode$ NoResize=�65    � 	Resources 5	         CustomButtonStyle �� 
TargetType �� Property �� Value3  -System.Windows.Media.Effects.DropShadowEffect   Color   ShadowDepth   
BlurRadius	 �� Triggers
 P� Property P� Value  LessImportantButtonStyle  DescriptionLabelStyle  HintLabelStyle��%  &	        &	 
    &	     &	 �    �� 5
   
   " ��6*   .�5      ��  � ��$	 Top=�62   �� 5       � ��$
 Left=�64   �� 5       � �� �	#FF3498DB6+   �� 5       � ��
 �����6+   �� 5       � ��$ 16(�6)   �� 5       � ��$
 10,5q�6(   �� 5       � ��$ 0q�60   �� 5       �@J�Effect 5        5      $ 	#33000000��6'   $ 2Q�69   $ 5Q�6I   5      	 5      P� 5      
 �@J�IsMouseOver . . �5      ��  � �� �#3C82B563   5      5      �� 5   
   " ��61   .�5      ��  � ��$	 Top=�62   �� 5       � ��$
 Left=�64   �� 5        � �� �	#FFE6C8D96+   �� 5!       � ��
 �����6+   �� 5"       � ��$ 16(�6)   �� 5#       � ��$
 10,5q�6(   �� 5$       � ��$ 0q�60   �� 5%       �@J�Effect 5&        5'      $ 	#33000000��6'   $ 2Q�69   $ 5Q�6I   5)      	 5*      P� 5+      
 �@J�IsMouseOver . . �5,      ��  � �� �	#FFCCB5C663   5-      5/      �� 50   
   " ��6.   .�51      ��  � �� �#6666666+   �� 52       � ��$ 12(�6)   �� 53       � ��$	 Top=�62   �� 54       � ��$
 Left=�64   55      �� 56   
   " ��6'   .�57      ��  � �� �	#FF5555556+   �� 58       � ��
 � E��6+   �� 59       � ��$ 12(�6)   �� 5:       � y�$ Hidden=�6+   �� 5;       � ��$	 Top=�62   �� 5<       � ��$
 Left=�64   5=      5?      .��5@      � $��0,0,4,0q�6    � ColumnDefinitions 5A   
   �� 5B      $
��Auto �6   �� 5C      .+�5E   
   �� -   5         ��Name$ allLEDScreen��5E      $)��#Envoyer a tout les ecran LED dispo?��6)   $��
61,343,0,0q�6X   $��2��6�   �� 5H   
   -   5         ��Name$ bluetoothDevicesListBox��5H      $	��221��64   $	��248��6B   $��40,87,638.577,63q�6N   6h   �� 5I   
   -   5        $ 
historique��5I      $	��230��6&   $��635,25,81.577,116q�63   $	��210��6N   ��5J      �� 5K      " ��6   .�5L      -   5        5M      5O      �� 5R   
   -   5         ��Name$	 tag��5R      $��22��6    $��323,268,483.577,81q�6,    �� TextWrapping$
 Wrap=�6H   $	��120��6]   6i   �� 5S   
   -   5        $ 	tagFilter��5S      $��23��6&   $��741,331,81.577,17q�62   $
 Wrap=�6M   $	��104��6b   6n   �� 5V   
   -   5         ��Name$ start��5V      $��Search Bluetooth Devices��6    $	��248��6E   $��	40,22,0,0q�6Q   $��46��6{   #��[  6�   �� 5W   
   -   5        $ sendMessage��5W      $��Send Messages��6&   $	��132��6@   $��407,308,0,0q�6L   $��46��6a   {�.  #��[  6�   �� 5X   
   -	   5        $��Date de création��5X      $��653,255,0,0q�6-   $	��174��6C   $��33��6O   #��[ 6{   �� 5Y   
   -
   5        $��Date d'utilisation��5Y      $	��174��60   $��653,293,0,0q�6<   $��33��6n   #��[ 6z   �� 5\   
   #��[ 6   $&�� Choisir un Tag pour les messages��6?   $��323,237,0,0q�6k   �� 5]   
   #��[ 6   $��Historique des messages��6?   $��
667,-1,0,0q�6b   �� 5^   
   #��[ 6   $��Filter par Tags��6?   $��635,331,0,0q�6Z   $��30��6p   $	��104��6|   �� 5`   
   -   5         ��Name$ errorMessage��5`   8   #��[ 6   $%��Pas d'écrans Led sélectionné��6N   $��456,263,0,0q�6v   $	��162��6�   $��29��6�   $y�Hidden=�6�   �� 5a   
   -   5        $ 	noDevices��5a   8   #��[ 6   $ ��Pas d'appareils diponibles��6K   $��
83,308,0,0q�6q   $��30��6�   $	��149��6�   $y�Hidden=�6�   � 5d   
   $	��270��6   $	��207��6   $��323,25,333.577,139q�6(    5e      �� 5f      $	��150 �6#   �� 5g      $	��120 �6#    � RowDefinitions 5j      �� 5k      $��23 �6    �� 5l      $��23 �6    �� 5m      $��23 �6    �� 5n      $��23 �6    �� 5o      $��23 �6    �� 5p      $��23 �6    �� 5q      $��23 �6    �� 5r      $��23 �6    �� 5s      $��23 �6    .+�5v      �� -   5        $ firstMessage��5v      $��23��6-   $
 Wrap=�69   $	��120��6N   $��1��6}   $��1��6�    J� 	AllowDrop . 6�   �� 5w      -   5        $ secondMessage��5w      $��23��6.   $
 Wrap=�6:   $	��120��6O    �� 
IsReadOnly . $��1��6�   $��2��6�    . 6�   �� 5x      -   5        $ thirdMessage��5x      $��23��6-   $
 Wrap=�69   $	��120��6N    . $��1��6�   $��3��6�    . 6�   �� 5y      -   5        $ forthMessage��5y      $��23��6-   $
 Wrap=�69   $	��120��6N    . $��1��6�   $��4��6�    . 6�   �� 5z      -   5        $ 	_5Message��5z      $��23��6*   $
 Wrap=�66   $	��120��6K    . $��1��6�   $��5��6�    . 6�   �� 5{      -   5        $ 	_6Message��5{      $��23��6*   $
 Wrap=�66   $	��120��6K    . $��1��6�   $��6��6�    . 6�   �� 5|      -   5        $ 	_7Message��5|      $��23��6*   $
 Wrap=�66   $	��120��6K    . $��1��6�   $��7��6�    . 6�   �� 5}      -   5        $ 	_8Message��5}      $��23��6*   $
 Wrap=�66   $	��120��6K    . $��1��6�   $��8��6�    . 6�   �� 5      #��[ 6   $��Numéro de message��6C   $	��151��6_   $��0��6k   $��0��6{   �� 5�      #��[ 6   $��Messages��6C   $	��103��6V   $��1��6b   $��0��6r   �� 5�      #��[ 6   $��1��6C   $	��103��6Q   $��0��6]   $��1��6m   �� 5�      #��[ 6   $��1��6C   $	��103��6Q   $��0��6]   $��1��6m   �� 5�      #��[ 6   $��2��6C   $	��103��6Q   $��0��6]   $��2��6m   �� 5�      #��[ 6   $��3��6C   $	��103��6Q   $��0��6]   $��3��6m   �� 5�      #��[ 6   $��4��6C   $	��103��6Q   $��0��6]   $��4��6m   �� 5�      #��[ 6   $��5��6C   $	��103��6Q   $��0��6]   $��5��6m   �� 5�      #��[ 6   $��6��6C   $	��103��6Q   $��0��6]   $��6��6m   �� 5�      #��[ 6   $��7��6C   $	��103��6Q   $��0��6]   $��7��6m   �� 5�      #��[ 6   $��8��6C   $	��103��6Q   $��0��6]   $��8��6m   5�      5�      5�      