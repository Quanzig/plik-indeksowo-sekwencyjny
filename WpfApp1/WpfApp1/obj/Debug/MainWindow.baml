   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	     � Title$  
MainWindow��5   	   $	��350��6   $	��525��6)   .��5	      � .+�5
   
   �� -   5         ��Name$ addRecordButton��5
      $��Dodaj��6*   $
��Left=�6:   $��	10,35,0,0q�6U   $	��Top=�6h   $��75��6�   6�   �� 5   
   -   5        $ showFileButton��5      $��Wyswietl��6)   $
��Left=�6<   $��
432,41,0,0q�6W   $	��Top=�6k   $��75��6�   6�   �� 5   
   -   5        $ resetFileButton��5      $
��Nowy��6*   $
��Left=�69   $��	10,10,0,0q�6T   $	��Top=�6g   $��75��6   6�   �� 5   
   -   5        $ reorganizeButton��5      $��Reorganizuj��6+   $
��Left=�6A   $��
370,96,0,0q�6\   $	��Top=�6p   $��75��6�   6�   �� 5   
   -   5         ��Name$ 
keyTextBox��5      $
��Left=�6&   $��23��6A   $��
10,141,0,0q�6M    �� TextWrapping$
 Wrap=�6a   $	��Top=�6u   $	��120��6�   �� 5   
   -   5        $ searchRecordButton��5      $��Szukaj��6-   $
��Left=�6>   $��168,144,0,0q�6Y   $	��Top=�6n   $��75��6�   6�   �� 5   
   $��Podaj klucz do wyszukania��6   $
��Left=�64   $��
10,110,0,0q�6O   $	��Top=�6c    J� RenderTransformOrigin$ -0.737,0.192&�6{   $	��160��6�   �� 5   
   -   5        $ deleteKeyTextBox��5      $
��Left=�6,   $��23��6G   $��
10,200,0,0q�6S   $
 Wrap=�6g   $	��Top=�6{   $	��120��6�   �� 5   
   -   5        $ deleteRecordButton��5      $
��Usun��6-   $
��Left=�6<   $��168,203,0,0q�6W   $	��Top=�6l   $��75��6�   6�   �� 5   
   $��Podaj klucz do usuniecia��6   $
��Left=�63   $��
10,169,0,0q�6N   $	��Top=�6b   $ -0.737,0.192&�6z   $	��160��6�   �� 5   
   -	   5        $ editRecordTextBox��5      $
��Left=�6-   $��23��6H   $��
10,254,0,0q�6T   $
 Wrap=�6h   $	��Top=�6|   $	��120��6�   �� 5   
   -
   5        $ 
editButton��5      $��Edytuj��6%   $
��Left=�66   $��168,257,0,0q�6Q   $	��Top=�6f   $��75��6~   6�   �� 5   
   $$��Podaj klucz elementu do edycji��6   $
��Left=�69   $��
10,223,0,0q�6T   $	��Top=�6h   $ -0.737,0.192&�6�   $	��205��6�   �� 5   
   $��Dodaj pojedynczy rekord��6   $
��Left=�62   $��
250,10,0,0q�6M   $	��Top=�6a   $	��159��6y   �� 5   
   -   5        $ addManuallyRecordButton��5      $��Dodaj��62   $
��Left=�6B   $��
250,41,0,0q�6]   $	��Top=�6q   $��75��6�   6�   �� 5   
   -   5        $ zapiszIndexButton��5      $��Zapisz��6,   $
��Left=�6=   $��
140,41,0,0q�6X   $	��Top=�6l   $��75��6�   6�   �� 5   
   $��Zapisz indeks��6   $
��Left=�6(   $��
140,12,0,0q�6C   $	��Top=�6W   5      5      