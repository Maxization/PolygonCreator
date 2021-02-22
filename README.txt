KLAWISZOLOGIA:
W celu edycji trzeba zaznaczyć odpowiedni radiobutton i póżniej nacisnąć lewym przyciskiem myszy na element, który chcemy edytować.
W celu nałożenia relacji trzeba nacisnąć prawy przycisk myszy na odpowiedni element, wtedy pojawi się contextMenu związane z relacjami.

ZAŁOŻENIA I OPIS ALGORYTMU:
Założyłem że relacja na wierzchołek to tak na prawdę relacja na dwie sąsiadujące krawędzie tego wierzchołka, oraz że relacja krawędzi "porusza się" po dwóch prostych.
Predefiniowany wielokąt wczytywany jest z pliku "polygon", który znajduje się w głównym folderze projektu.

Algorytm realacji polega na tym że Wielokąt posiada listę relacji które są na niego nałożone, kiedy chcemy poruszyć wierzchołkiem/krawędzią to poruszam nią,
a następnie naprawiam relacje którę tego wymagają, powtarzam tę operację rekurencyjnie za każdym razem kiedy zdarzy sie że kolejne relacje zostają zaburzone,
a po każdym takim naprawieniu relacji usuwam ją tymczasowo aby nie doszło do zapętlenia.