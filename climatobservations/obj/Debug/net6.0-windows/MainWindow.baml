   M S B A M L   `   `   `  ����M  Iclimatobservations, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:climatobservationsclimatobservations  5   	   #    climatobservations.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:climatobservations     5   	     � Title$  
MainWindow��5   	   $	��456��6   $	��816��6)   .��5	      � $��	0,0,20,11q�6    � ColumnDefinitions 5
   
   �� 5      $
��194* �6   �� 5      $��5* �6   .+�5   
   �� -   5         ��Name$ txtNameObserver��5      $
��Left=�6+   $��	53,42,0,0q�6F   $�� ��6Y    �� TextWrapping$
 Wrap=�6a   $	��Top=�6u   $	��120��6�   $��18��6�   �� 5   
   $��Namn på observatör��6   $
��Left=�6-   $��	47,11,0,0q�6H   $	��Top=�6[   $��26��6s   $	��119��6   �� 5   
   -   5         ��Name$ btnAddObserver��5      $��
Lägg till��6)   $
��Left=�6=   $��	54,73,0,0q�6X   $	��Top=�6k   $��50��6�   $��20��6�   �� 5   
   -   5        $ btnRemoveObserver��5      $��Ta bort��6,   $
��Left=�6>   $��	54,98,0,0q�6Y   $	��Top=�6l   $��50��6�   $��20��6�   �� 5   
   -   5         ��Name$ lstbox��5      $��491,41,34,303q�6"   6:   �� 5   
   $��Presentera observatör��6   $
��Left=�60   $��
488,12,0,0q�6K   $	��Top=�6_   $��26��6w   $	��126��6�   �� 5   
   -   5        $ btnShowObserver��5      $��Visa observatör��6*   $
��Left=�6D   $��492,129,0,0q�6_   $	��Top=�6t   $	��125��6�   $��20��6�   �� 5   
   -   5        $ txtMeasurement��5      $
��Left=�6*   $��500,338,0,0q�6E   $
 Wrap=�6Z   $5��/Ange antal i mått, styck, celsius i siffror...��6n   $	��Top=�6�   $	��245��6�   $y�Hidden=�6�   $��18��6�   �� 5   
   -   5         ��Name$	 lbl��5      $��Registrera observation��6   $
��Left=�6>   $��494,164,0,0q�6Y   $	��Top=�6n   $y�Hidden=�6�   $��26��6�   $	��129��6�   �� 5   
   -   5        $ btnAddObservation��5      $��Lägg till observation��6,   $
��Left=�6L   $��502,387,0,0q�6g   $	��Top=�6|   $	��124��6�   $y�Hidden=�6�   $��20��6�   �� 5   
   -	   5        $ lstboxCategory��5      $��495,187,31,99q�6*   $y�Hidden=�6A   6U   �� 5   
   -
   5        $ txtMeasurementSnow��5      $
��Left=�6.   $��499,361,0,0q�6I   $
 Wrap=�6^   $��Ange snödjup i cm... ��6r   $	��Top=�6�   $	��246��6�   $y�Hidden=�6�   $��18��6�   �� 5   
   -   5        $ lstboxObservation��5      $��54,189,392,34q�6-   6D   �� 5   
   $��Presentera observationer��6   $
��Left=�63   $��
54,161,0,0q�6N   $	��Top=�6b   $��26��6z   $	��142��6�   5      5      