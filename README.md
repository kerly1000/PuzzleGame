# .net MAUI pusle mäng PuzzleGame 

Mäng on .NET MAUI raamistikuga loodud C# keeles rakendus, mis pakub kasutajale mõtlemisosavust treenivat ajaviidet. Mängija ülesanne on liigutada kindlaksmääratud aja jooksul pusletükid õigesse asukohta, et moodustada terviklik pilt.
Mängus on erinevad funktsioonid: pildi valimine telefoni galeriist, raskusastme valimine, pusletükkide asukoha vahetamine, et neid õigesse kohta paigutada ning pilt kokku panna, kasutada tuleb nii kiirust kui tähelepanu, pildi kokkusaamisel tuvastab süsteem, mitu käiku tehti ning milline ajavaru jäi alles.
Funktsionaalsus

Mängu arendamisel on kasutatud tehnoloogiaid:
.NET MAUI – rakenduse arendamiseks.
C# – mängu loogika programmeerimiseks.
XAML – kasutajaliidese loomiseks.
Visual Studio Insiders – rakenduse arendamiseks ja emulaatoriga testimiseks.
Samsung androidtelefon- rakenduse testimiseks.


Rakenduse käivitamiseks on vaja:
Visual Studiot koos .NET MAUI arendustööriistadega.
Sobivat .NET SDK versiooni.
Projekti töölerakendamiseks vastavat seadet või emulaatorit.

Käivitamine:
Klooni projekt või laadi lähtekood alla.
Ava projekti .slnx fail Visual Studios.
Taasta vajalikud NuGeti paketid.
Vali käivitusplatvorm Androidi emulaator või ühenda androidseade andmeedastuskaabliga arendaja režiimis.
Käivita rakendus vajutades Run või F5.

Projekti struktuur:
PuzzleGame/
├── Models/ 
    ├── GameResult.cs
    └── Puzzle.cs
├── Pages/
    ├── AddPuzzlePage.xaml
    ├── AddPuzzlePage.xaml.cs
    ├── PuzzleGamePage.xaml
    ├── PuzzleGamePage.xaml.cs
    ├── PuzzleSetupPage.xaml
    ├── PuzzleSetupPage.xaml.cs
    ├── ResultsPage.xaml
    └── ResultsPage.xaml.cs
├── Services             # Teenuse liidesed
    ├── PuzzleImageService.cs
    └── PuzzleService.cs
├── MainPage.xaml        # Avalehe kasutajaliides
├── MainPage.xaml.cs     # Avalehe loogika
├── App.xaml             # Rakenduse ressursid
├── App.xaml.cs          # Rakenduse käivitamine
└── PuzzleGame.csproj    # Projekti konfiguratsioon
