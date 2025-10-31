# OPG Robin Strandberg SYSM9


* Kort sammanfattning 1 sida, flytande text

CRUD-applikation med Create, Read, Update och Delete med WPF. Användarregistrering i en "CookMaster-App"
där användare kan registrera sig, återställa lösenord i "forgot password" och logga in. Vid inloggning överblicka
receptlista och navigera vidare till "user details", "recipe details" och "add recipe".

* Sammanfattning och analys av projektets struktur och uppbyggnad

Projektets struktur är skapad i MVVM-arkitektur. Projektet innehåller managers, models, themes, viewmodels och views.
Inline-styles har bytts mot globalt tema för congruent UI-design. Stora utmaningar och mycket tid har lagts ned på att hålla
isär logik mellan "managers" och viewmodels som nyttjar managers metoder. Managers metoder har eftersträvats att hållas
så "rena" som möjligt där enbart åtgärden i sig utförs (i något enstaka fall har logiken till viss del överlappat).

* Fördelar och nackdelar med olika approacher 

- Inbäddning med (WPF) UserControls valdes i applikation för att undvika att nya fönster öppnas upp för användaren.
Nya fönster som öppnas (oväntat) ovanpå befintlig applikationssida bör undvikas ur ett användarvänligt perspektiv (om det 
inte anges explicit e.g. "Öppna i nytt fönster"). Öppnandet av nya fönster kan inge ett oprofessionelt intryck
med oväntade pop-up fönster. OBS! Forgotpasswordwindow och registerwindow förblev egna fönster pga svårigheter att bädda 
in ytterligare en sektion (bestående av register och forgotpassword) i den första loginsektionen.

- Vid val av global resurs av objektinstansen user manager valdes användande av en statisk resurs 
i code-behind app.xaml. Detta medförde en fördel i att resursen enklare kunde nås genom den kortare
referensen App.UserManager (färdigkompilerad resurs) istället för en längre referens till global
resurs, enbart globalt initierad i app.xaml (Current.Resource["UserMan...]).

En recipe manager skapas vid varje lyckad inloggning per användare. Detta medförde ett större integritetsskydd
för användaren om ytterligare logik för cachning och historik behövs läggas till.


