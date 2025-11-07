# OPG Robin Strandberg SYSM9


* Kort sammanfattning 1 sida, flytande text

CRUD-applikation med Create, Read, Update och Delete med WPF. Användarregistrering i en "CookMaster-App"
där användare kan registrera sig, återställa lösenord i "forgot password" och logga in. Vid inloggning överblicka
receptlista och därifrån kan användaren navigera vidare till "user details", "recipe details" och "add recipe"-fönster.

* Sammanfattning och analys av projektets struktur och uppbyggnad

Projektets struktur är skapad i MVVM-arkitektur. Projektet innehåller managers, models, themes, viewmodels och views.
Inline-styles har bytts mot globalt tema för congruent UI-design. Stora utmaningar och mycket tid har lagts ned på att hålla
isär logik mellan "managers" och viewmodels som nyttjar managers och modellers metoder. Managers metoder har eftersträvats att hållas
så "rena" som möjligt där enbart åtgärden i sig utförs och återfinns i modellerna. Detta med fördel för renare och mer överblickbara 
viewmodels.

* Fördelar och nackdelar med olika approacher 

- Inbäddning med (WPF) UserControls valdes inledningsvis i applikationen för att undvika att nya fönster öppnas upp för användaren. Denna struktur bestod av 
inloggningsektion med registering, glömt lösenord och login-funktionalitet som sedan gömdes vid lyckas inloggning, och istället visades resterande vyer för användaren.
Nya fönster som öppnas (oväntat) ovanpå befintlig applikationssida kan undvikas ur ett användarvänligt perspektiv (om det 
inte anges explicit e.g. "Öppna i nytt fönster"). Öppnandet av nya fönster kan inge ett störande intryck
med oväntade pop-up fönster. Forgotpasswordwindow och registerwindow förblev egna fönster i detta skede pga svårigheter att bädda 
in ytterligare en barn-sektion (bestående av register och forgotpassword) inuti den första loginsektionen. Sedermera övergavs användandet av user controls
överallt då det krävdes mycket felsökning för att få gränssnittet att fungera. Således kan slutsatsen dras att fönster istället för inbäddning är en traditionell 
och pålitlig metod som håller isär olika komponenter i gränssnittet effektivt. Nackdelen är fönster som öppnas på varandra vilket ger kan ge visst vilseledande intryck för
användaren.

- Vid val av global resurser och av objektinstansen user manager valdes användande av OnStartUp-metoden (ärvd från application-klassen)
i code-behind app.xaml. Här placerades även det globala temat, en global felhanterare för UI-tråden och en global felhanteraren för bakgrundstrådar. 
Detta medförde en fördel i att resurserna initieras/laddas efter app-klassen är kompilerad (till g.cs) och valdes
pga startup-uri användes initialt i app.xaml (och kördes direkt vid uppstart tillsammans med user manager) vilket skapas synkroniseringsproblem
mellan mainwindow VM, som kastade fel i konstruktorn då globala resurser inte kunde hittas vid just detta debug-tillfälle. Därav valdes med fördel
en mer holistik approach där startup, user manager, tema och globala felhanterare sätts i code-behind app och körs enbart när app-klassen är färdigkompilerad.

- En instans av recipe manager-klassen valdes att skapas vid varje lyckad inloggning per användare. Detta medförde ett större integritetsskydd
för användaren om ytterligare logik för cachning och historik behövs läggas till. Exempel för detta kan vara tidigare sökningar eller 
åtgärder utförda med varje användares inviduella recipe manager (recepthanterare). 

- Även ShutDownMode i app.xaml.cs sattes till OnExplicitShuwdown för att undivka att applikationen stängs om alla fönster råkas stängas av användaren.
Tillagda recept, användare undviks därav att raderas om användaren skulle råka stänga samtliga fönster. Vidare implementerades en lyssnar-metod om alla fönster stängs
för att ge användaren möjlighet att öppna appen igen om denne skulle råka stänga alla aktiva fönster (även om appen fortsätter köras).

- Tvåfaktorsidentifering (2FA) implementerades slutligen med simulerad kod till mejl som visas i messagebox, nytt 2FA fönster för inmatning och jämförelse
med genererad autentiseringskod (direkt jämförelse mot textbox innehåll i vyn). If-logik implementerades även i login-metoden som bool för kontroll av lyckad
2FA.
