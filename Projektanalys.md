# OPG Robin Strandberg SYSM9


* Kort sammanfattning 1 sida, flytande text

CRUD-applikation med Create, Read, Update och Delete med WPF. Användarregistrering i en "CookMaster-App"
där användare kan registrera sig, återställa lösenord i "forgot password" och logga in. Vid inloggning överblicka
receptlista och därifrån kan användaren navigera vidare till "user details", "recipe details" och "add recipe"-fönster.

* Sammanfattning och analys av projektets struktur och uppbyggnad

Projektets struktur är skapad i MVVM-arkitektur. Projektet innehåller managers, models, themes, viewmodels och views.
Inline-styles har bytts mot globalt tema för congruent UI-design. Stora utmaningar och mycket tid har lagts ned på att hålla
isär logik mellan "managers" och viewmodels som nyttjar managers metoder. Managers metoder har eftersträvats att hållas
så "rena" som möjligt där enbart åtgärden i sig utförs (i något enstaka fall har logiken till viss del överlappat).

* Fördelar och nackdelar med olika approacher 

- Inbäddning med (WPF) UserControls valdes inledningsvis i applikationen för att undvika att nya fönster öppnas upp för användaren.
Nya fönster som öppnas (oväntat) ovanpå befintlig applikationssida kan undvikas ur ett användarvänligt perspektiv (om det 
inte anges explicit e.g. "Öppna i nytt fönster"). Öppnandet av nya fönster kan inge ett oprofessionelt intryck
med oväntade pop-up fönster. OBS! Forgotpasswordwindow och registerwindow förblev egna fönster pga svårigheter att bädda 
in ytterligare en sektion (bestående av register och forgotpassword) inuti den första loginsektionen. Sedermera övergavs användandet user controls
överallt de det krävdes mycket felsökning för att få gränssnittet att fungera. Således kan slutsatsen dras att fönster istället för inbäddning är en traditionell metod 
och pålitlig metod som håller isär olika komponenter i gränssnittet effektivt. Nackdelen är fönster som öppnas på varandra vilket ger kan ge visst vilseledande intryck för
användaren.

- Vid val av global resurs av objektinstansen user manager valdes användande av en OnStartUp-metoden 
i code-behind app.xaml. Detta medförde en fördel i att resurserna initieras/laddas efter app-klassen är kompilerad och valdes
pga startup-uri användes initialt i app (och kördes direkt vid uppstart tillsammans med user manager) vilket skapas synkroniseringsproblem
mellan mainwindow VM som kastade fel i konstruktorn då globala resurser inte kunde hittas vid just detta debug-tillfälle. Därav valdes med fördel
en mer holistik approach där startup och user manager sätts i code-behind app och körs enbart när app-klassen är färdigkompilerad.

En instans av recipe manager-klassen skapas vid varje lyckad inloggning per användare. Detta medförde ett större integritetsskydd
för användaren om ytterligare logik för cachning och historik behövs läggas till. Exempel för detta kan vara tidigare sökningar eller 
åtgärder utförda med varje användares inviduella recipe manager (recepthanterare).

- Med fördel valdes att applikationen inte stängs om alla fönster stängs då användardata, tillagda recept, användare etc
inte bör raderas om användaren skulle stänga fönstret (ShutdownMode.OnExplicitShutdown). 
Applikationen i ett praktiskt scenario körs då via extern server.
