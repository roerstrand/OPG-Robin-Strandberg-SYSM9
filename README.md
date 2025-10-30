# OPG Robin Strandberg SYSM9

<!--  

* Kort sammanfattning 1 sida, flytande text

CRUD-applikation med Create, Read, Update och Delete med WPF och användarregistrering.

* Sammanfattning och analys av projektets struktur och uppbyggnad

- Embedding med (WPF) UserControls valdes i startsida för att undvika att nya fönster öppnas upp för användaren.
Nya fönster som öppnas (oväntat) ovanpå befintlig applikationssida bör undvikas ur ett användarvänligt perspektiv (om det 
inte anges explicit e.g. "Öppna i nytt fönster"). Öppnandet av nya fönster kan inge ett oprofessionelt intryck
med oväntade pop-up fönster.

* Fördelar och nackdelar med olika approacher 

Vid val av global resurs av objektinstansen user manager valdes användande av en statisk resurs 
i code-behind app.xaml. Detta medförde en fördel i att resursen enklare kunde nås genom den kortare
referensen App.UserManager (färdigkompilerad resurs) istället för en längre referens till global
resurs, enbart globalt initierad i app.xaml (Current.Resource["UserMan...]).

I mainwindow code-behind valdes även att lagra DataContext i en mainwindowviewmodel-klass
(_viewModel) som på ett liknande sätt skapade en enklare referering från sändar-elementet i vyn 
(PasswordBox) till mottagande egenskap i VM för mainwindow (PasswordInput). Detta då direkt
binding till VM egenskaper inte stöds via Passwordbox.

-->
