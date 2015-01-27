using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RustRconManager
{
    class Language
    {
        public static readonly int MAX_LEN = 100;
        public static readonly int GERMAN = 1;
        public static readonly int ENGLISH = 2;

        private static Dictionary<string, string> colours = new Dictionary<string, string>()
        {
            {"normal", "#BBBBBB"},
            {"red", "#FF0000"},
            {"green", "#00FF00"},

            {"pink", "#FF66FF"},
            {"bronze", "#CD7F32"},
            {"silver", "#BCC6CC"},
            {"gold", "#FDD017"},
            {"platin", "#858482"},
            {"diamond", "#81DAF5"}
        };

        public static void init()
        {
            language = new Dictionary<string, string[]>()
        {
            // Timeouts
            {"validate", new string[] { "[red]ACHTUNG! [normal]Gebe diesen Befehl in [red]{0} [normal]erneut ein um ihn zu aktivieren.", "[red]ATTENTION! [normal]Enter this command again in [red]{0} [normal]to activate it." }},
            {"wait", new string[] { "Bitte warte weitere [red]{0} [normal]und gebe den Befehl erneut ein.", "Please wait another [red]{0} [normal]and then type the command again." }},
            {"late", new string[] { "Du hast [red]zu lange gewartet [normal]um den Befehl einzugeben! Versuche es erneut.", "You [red]waited too long [normal]to enter the command! Try again!" }},
            {"cmdtime", new string[] { "{0}{1} [normal]kann diesen Befehl erst wieder in [red]{2} [normal]verwenden.", "{0}{1} [normal]can use this command again in [red]{2}[normal]." }},

            // Restrictions
            {"viponly", new string[] { "Nur [red]VIPs [normal]können das!", "Only [red]VIPs [normal]can do that!" }},
            {"noneonly", new string[] { "Du bist {0}VIP [normal]und brauchst das nicht mehr!", "You are a {0}VIP [normal]and don't need that anymore!" }},
            {"noperm", new string[] { "Du hast nicht die benötigten Rechte.", "You do not have the required permissions." }},

            // User traffic
            {"join", new string[] { "{0}{1} [normal]besucht uns aus [red]{2}[normal].", "{0}{1} [normal]visits us from [red]{2}[normal]." }},
            {"quit", new string[] { "{0}{1} [normal]hat uns [red]verlassen[normal].", "{0}{1} [normal]has [red]left [normal]us." }},
            
            {"blacklist", new string[] { "Kicke [red]{0}[normal]: [red]{1}[normal] liegt nicht in Europa.", "Kicking [red]{0}[normal]: [red]{1}[normal] is not in Europe." }},

            {"firstwelcome", new string[] { "Herzlich willkommen, [pink]{0}[normal].", "Welcome, [pink]{0}[normal]." }},
            {"firstgreeting", new string[] { "Das ist hier dein erstes Mal. Gebe [red]/hilfe [normal]ein oder besuche [red]http://bloodisgood.org/rust[normal].", "This is your first time here. Enter [red]/help [normal]or visit [red]http://bloodisgood.org/rust[normal]."}},

            {"welcome", new string[] { "Willkommen zurück, {0}{1}[normal].", "Welcome back, {0}{1}[normal]." }},
            
            {"tabout", new string[] { "{0}{1} [normal]ist grade nicht da.", "{0}{1} [normal]is now away from keyboard." }},
            {"tabin", new string[] { "{0}{1} [normal]ist wieder da.", "{0}{1} [normal]is back again." }},
            {"timeout", new string[] { "{0}{1} [normal]hat die Verbindung verloren.", "{0}{1} [normal]has timed out." }},

            {"kick-name", new string[] { "Kicke [red]{0}[normal]: Dieser Name ist bereits in Verwendung.", "Kicking [red]{0}[normal]: This name is already taken." }},
            {"nick-change", new string[] { "{0}{1} [normal]heisst nun {0}{2}[normal].", "{0}{1} [normal]is now called {0}{2}[normal]." }},

            {"karma-5", new string[] { "[red]Achtung! {0}{1} [normal]ist potenziell gefählich!", "[red]Attention! {0}{1} [normal]is probably dangerous!" }},
            {"karma-10", new string[] { "[red]Achtung! {0}{1} [normal]ist [red]gefährlich[normal]!", "[red]Attention! {0}{1} [normal]is [red]dangerous[normal]!" }},
            {"karma-20", new string[] { "[red]ACHTUNG! {0}{1} [normal]ist [red]wirklich gefährlich[normal]!", "[red]ATTENTION! {0}{1} [normal]is [red]really dangerous[normal]!" }},
            {"karma-50", new string[] { "[red]ACTHUNG! {0}{1} [normal]wird dich [red]töten[normal]! Daran besteht kein Zweifel.", "[red]ATTENTION! {0}{1} [normal]will [red]kill [normal]you! There is no doubt about it." }},

            {"message", new string[] { "{0}{1}[normal], es wurde eine Nachricht für dich von {2}{3} [normal]hinterlassen:", "{0}{1}[normal], a message has been left for you by {2}{3}[normal]:" }},

            {"vacd", new string[] { "[red]{0} [normal]wurde gebannt wegen [red]{1} [normal]VAC bans in den letzten [red]{2} [normal]Tagen.", "[red]{0} [normal]was banned for having [red]{1} [normal]VAC bans from the last [red]{2} [normal]days." }},
            {"vacok", new string[] { "{0}{1} [normal]hat [red]{2} [normal]VAC bans vor [red]{3} [normal]Tagen. Wir drücken da mal ein Auge zu.", "{0}{1} [normal]has [red]{2} [normal]VAC bans from [red]{3} [normal]days ago. We'll give you a chance." }},

            {"suicided", new string[] { "{0}{1} [normal]hat sich [red]umgebracht[normal].", "{0}{1} [normal]has [red]suicided[normal]." }},

            {"violation", new string[] { "{0}{1} [normal]hat etwas seltsames gemacht ([red]{2}[normal]) und wurde deswegen gekickt.", "{0}{1} [normal]did something strange ([red]{2}[normal]) and was kicked because of that." }},

            {"ticket", new string[] { "Steam hat das Ticket von {0}{1} [normal]wiederrufen.", "Steam has cancelled the Ticket of {0}{1}[normal]." }},

            {"entry", new string[] { "{0}{1} [normal]ist schon verbunden, der Zugang wurde verwehrt.", "{0}{1} [normal]is already connected, the entry has been denied." }},

            // VIP
            {"vip-Bronze-grats", new string[] { "[red]Gratulation [bronze]{0}[normal], du hast [red]10 Stunden [normal]hier verbracht und bist nun [bronze]Bronze VIP[normal]!", "[red]Congratulations [bronze]{0}[normal], you have spent [red]10 hours [normal]here and are now [bronze]bronze VIP[normal]!" }},
            {"vip-Silver-grats", new string[] { "[red]Gratulation [silver]{0}[normal], du hast [red]20 Stunden [normal]hier verbracht und bist nun [silver]Silber VIP[normal]!", "[red]Congratulations [silver]{0}[normal], you have spent [red]20 hours [normal]here and are now [silver]silver VIP[normal]!" }},
            {"vip-Gold-grats", new string[] { "[red]Gratulation [gold]{0}[normal], du hast [red]50 Stunden [normal]hier verbracht und bist nun [gold]Gold VIP[normal]!", "[red]Congratulations [gold]{0}[normal], you have spent [red]50 hours [normal]here and are now [gold]gold VIP[normal]!" }},
            {"vip-Platinum-grats", new string[] { "[red]Gratulation [platin]{0}[normal], du hast [red]100 Stunden [normal]hier verbracht und bist nun [platin]Platin VIP[normal]!", "[red]Congratulations [platin]{0}[normal], you have spent [red]100 hours [normal]here and are now [platin]platinum VIP[normal]!" }},
            {"vip-Diamond-grats", new string[] { "[red]Gratulation [diamond]{0}[normal], du hast [red]200 Stunden [normal]hier verbracht und bist nun [diamond]Diamant VIP[normal]!", "[red]Congratulations [diamond]{0}[normal], you have spent [red]200 hours [normal]here and are now [diamond]diamond VIP[normal]!" }},

            {"donate-thanks", new string[] { "[red]Danke[normal], {0}{1}[normal], für deine Spende!", "[red]Thanks[normal], {0}{1}[normal], for your donation!" }},
            
            // Cron
            {"hourly", new string[] { "Es ist jetzt [red]{0} [normal]Uhr in Zentraleuropa.", "It's now [red]{0} [normal]o'clock in Central Europe." }},
            {"ighourly", new string[] { "Es ist jetzt [green]{0} [normal]Uhr in dieser Welt.", "It's now [green]{0} [normal]o'clock in this world." }},

            // Server
            {"save", new string[] { "Es wurden [red]{0} [normal]Objekte gespeichert." }},

            // General
            {"nomatch", new string[] { "Es konnte kein Spieler mit [red]{0} [normal]im Namen gefunden werden.", "No player with a name containing [red]{0} [normal]found." }},
            {"manymatch", new string[] { "Es wurden [red]{0} [normal]Spieler mit [red]{1} [normal]im Namen gefunden. Präzisiere deine Suche.", "There were [red]{0} [normal]players found with [red]{1} [normal]in their name. Please be more precise." }},
            {"noonline", new string[] { "Es ist kein Benutzer mit [red]{0} [normal]im Namen [green]online[normal].", "There is no user with [red]{0} [normal]in their name [green]online[normal]." }},
            {"offline", new string[] { "{0}{1} [normal]ist [red]offline[normal].", "{0}{1} [normal]is [red]offline[normal]." }},

            {"nogmatch", new string[] { "Es konnte keine Gruppe mit [red]{0} [normal]im Namen gefunden werden.", "No group with a name containing [red]{0} [normal]found." }},
            {"manygmatch", new string[] { "Es wurden [red]{0} [normal]Gruppen mit [red]{1} [normal]im Namen gefunden. Präzisiere deine Suche.", "There were [red]{0} [normal]gruops found with [red]{1} [normal]in their name. Please be more precise." }},
            {"noamatch", new string[] { "Es konnte keine Allianz mit [red]{0} [normal]im Namen gefunden werden.", "No alliance with a name containing [red]{0} [normal]found." }},
            {"manyamatch", new string[] { "Es wurden [red]{0} [normal]Allianzen mit [red]{1} [normal]im Namen gefunden. Präzisiere deine Suche.", "There were [red]{0} [normal]alliances found with [red]{1} [normal]in their name. Please be more precise." }},

            // Commands
            {"help1", new string[] { "Folgende Befehle stehen zur Verfügung:", "The following commands are available:" }},
            {"help2", new string[] { "[red]Unterstützung: [normal]kit, tpr, tpa, verlaufen, quit", "[red]Support: [normal]kit, tpr, tpa, lost, quit" }},
            {"help3", new string[] { "[red]Informationen: [normal]count, zeit, log, airdrop, wer, vip, ping, suicide, remove, teamspeak", "[red]Information: [normal]count, time, log, airdrop, vip, ping, suicide, remove, teamspeak" }},
            {"help4", new string[] { "[red]VIP: [normal]pvp, staat, votekick", "[red]VIP: [normal]pvp, state, votekick" }},
            {"help5", new string[] { "Benutzung und Weiteres auf [red]http://bloodisgood.org/rust", "Usage and more at [red]http://bloodisgood.org/rust" }},

            {"airdrop1", new string[] { "[red] 0-10: [normal]Kein Airdrop", "[red] 0-10: [normal]No airdrop" }},
            {"airdrop2", new string[] { "[red]11-20: [normal]Ein Airdrop alle 60 bis 90 Minuten", "[red]11-20: [normal]One airdrop every 60 to 90 Minutes" }},
            {"airdrop3", new string[] { "[red]21-50: [normal]Ein Airdrop alle 30 bis 45 Minuten", "[red]21-50: [normal]One airdrop every 30 to 45 Minutes" }},
            {"airdrop4", new string[] { "[red]50+  : [normal]Zwei Airdrops alle 30 bis 45 Minuten", "[red]50+  : [normal]Two airdrops every 30 to 45 Minutes" }},

            {"suicide", new string[] { "Du musst dich selbst töten! Drücke [red]F1[normal], gebe [red]suicide [normal]ein und bestätige mit Enter.", "You have to kill yourself. Press [red]F1[normal], type [red]suicide [normal]and press enter." }},
            
            {"remove", new string[] { "Es gibt kein Remove in vanilla Rust, du musst es selbst zerstören. Foundations, Pillars und Ceilings sind nicht zerstörbar.", "There is no remove in vanilla Rust, you have to destroy it yourself. Foundations, pillars and ceilings are indistructible." }},

            {"services1", new string[] { "[red]Mumble: [normal]mumble.bloodisgood.org", "[red]Mumble: [normal]mumble.bloodisgood.org" }},
            {"services2", new string[] { "[red]Teamspeak: [normal]teamspeak.bloodisgood.org", "[red]Teamspeak: [normal]teamspeak.bloodisgood.org" }},
            {"services3", new string[] { "[red]Website: [normal]http://bloodisgood.org/rust", "[red]Website: [normal]http://bloodisgood.org/rust" }},

            {"airdrop-incoming", new string[] { "[red]!!! A i r d r o p !!!", "" }},

            {"count", new string[] { "Es sind [red]{0} [normal]Spieler online.", "There are [red]{0} [normal]players online." }},
            {"alone", new string[] { "Du bist ganz alleine.", "You are all alone." }},

            {"nosleeper", new string[] { "[pink]{0} [normal]hat sich [red]ohne Sleeper [normal]ausgeloggt.", "[pink]{0} [normal]logged out [red]without sleeper[normal]." }},

            {"parachute-open", new string[] { "Fallschirm [green]aktiviert[normal]!", "Parachute [green]activated[normal]!" }},
            {"parachute-closed", new string[] { "Fallschirm [red]deaktiviert[normal]!", "Parachute [red]deactivated[normal]!" }},
            {"parachute-already", new string[] { "Ein Fallschirm wurde bereits aktiviert.", "A parachute has already been activated." }},

            {"who_on", new string[] { "{0}{1} [normal]([red]{2}[normal]) ist [green]online[normal], war seit [red]{3} [normal]hier und ist {0}{4}[normal].", "{0}{1} [normal]([red]{2}[normal]) is [green]online[normal], was [red]{3} [normal]here and is {0}{4}[normal]." }},
            {"who_off", new string[] { "{0}{1} [normal]([red]{2}[normal]) ist [red]offline[normal], war seit [red]{3} [normal]hier und ist {0}{4}[normal].", "{0}{1} [normal]([red]{2}[normal]) is [red]offline[normal], was [red]{3} [normal]here and is {0}{4}[normal]." }},
            {"who_last", new string[] { "{0}{1} [normal]wurde zuletzt am [red]{2} [normal]gesehen.", "{0}{1} [normal]was last seen [red]{2}[normal]." }},
            {"who_group", new string[] { "{0}{1} [normal]ist Mitglied bei [red]{2}[normal].", "{0}{1} [normal]is member of [red]{2}[normal]." }},

            {"time", new string[] { "Es ist [red]{0} [normal]in Zentraleuropa und [green]{1} [normal]in dieser Welt.", "It's [red]{0} [normal]in Central Europe and [green]{1} [normal]in this world." }},

            {"auth", new string[] { "Ein [red]{1} [normal]ist jetzt [green]online[normal]. Bei Problemen und Fragen, wende dich an [red]{0}[normal].", "A [red]{1} [normal]is now [green]online[normal]. For questions and help, ask [red]{0}[normal]." }},
            {"noauth", new string[] { "Das hättest du gerne.", "You wished." }},
            {"deauth", new string[] { "Abgemeldet.", "Deauthed." }},

            {"kit", new string[] { "Kit an {0}{1} [normal]ausgeliefert.", "Kit delivered to {0}{1}[normal]." }},
            {"authkit", new string[] { "[red]{2} [normal]kit an {0}{1} [normal]ausgeliefert.", "[red]{2} [normal]kit delivered to {0}{1}[normal]." }},
            {"nokit", new string[] { "Kit [red]{0} [normal]nicht gefunden.", "Couldn't find kit [red]{0}[normal]." }},
            {"whichkit", new string[] { "Welches Kit brauchst du?", "Which kit do you need?" }},

            {"tprhow", new string[] { "Verwendung: [red]/tpr Spielername [normal](Spielername kann ein Teil oder der gesamte Name sein).", "Usage: [red]/tpr Player name [normal](Player name can be a part or the whole name)." }},
            {"tprrequest", new string[] { "{0}{1}[normal], es möchte sich {2}{3} [normal]zu dir teleportieren. Akzeptiere mit [red]/tpa[normal].", "{0}{1}[normal], {2}{3} [normal]would like to teleport to you. Accept with [red]/tpa[normal]." }},
            {"tpanobody", new string[] { "Es möchte sich niemand zu dir teleportieren.", "Nobody wants to teleport to you." }},
            {"tpa", new string[] { "{0}{1} [normal]wurde zu {2}{3} [normal]bewegt.", "{0}{1} [normal]was moved to {2}{3}[normal]." }},

            {"rescued", new string[] { "{0}{1} [normal]wurde gerettet.", "Rescued {0}{1}[normal]." }},

            {"chatlog", new string[] { "{0}{1}[normal]: {2}", "Uh. Eh. What?" }},

            {"tpwhere", new string[] { "Soll ich erraten wo du hin möchtest oder was?", "Should I guess where you want to go or what?" }},
            {"tpdone", new string[] { "{0}{1} [normal]wurde teleportiert.", "{0}{1} [normal]was teleported." }},

            {"warn-1", new string[] { "{0}{1} [normal]wurde [red]{2} [normal]Mal Verwarnt. Mach so weiter und du bist nicht mehr lange hier.", "{0}{1} [normal]was warned [red]{2} [normal]times. Keep going like this and you won't be here for long." }},
            {"warn-3", new string[] { "{0}{1} [normal]wurde jetzt schon zum [red]dritten [normal]Mal verwarnt. Das nächste Mal gibt es einen [red]Ban[normal].", "{0}{1} [normal]has been warned for the [red]third [normal]time. Next time you will be [red]banned[normal]." }},
            {"warn-4", new string[] { "{0}{1} [normal]konnte sich einfach nicht beherrschen und wurde dafür [red]gebannt[normal].", "{0}{1} [normal]just couldn't contain him/her and was [red]banned [normal]because of that." }},
            {"unwarn", new string[] { "{0}{1} [normal]wurde eine Verwarnung abgezogen und hat jetzt [red]{2} [normal]Verwarnungen.", "One warning was removed from {0}{1} [normal]with [red]{2} [normal]remaining warnings." }},

            {"onauth", new string[] { "Es sind folgende [red]{0} [normal]Admins aktiv:", "The following [red]{0} [normal]admins are active:" }},
            {"offauth", new string[] { "Es sind keine Admins aktiv und folgende [red]{0} [normal]im Spiel:", "There are no admins active and the following [red]{0} [normal]in game." }},
            {"nobodyauth", new string[] { "Es sind keine Admins online. Benutze [red]/report [normal]um einen zu kontaktieren.", "There are no admins online. Use [red]/report [normal]to contact one." }},

            {"travel", new string[] { "In welche Stadt möchtest du reisen?", "To which town would you like to travel?" }},
            {"notravel", new string[] { "Konnte keine Stadt mit [red]{0} [normal]im Namen finden.", "Could not find a town with [red]{0} [normal]in the name." }},
            {"traveled", new string[] { "{0}{1} [normal]ist an einen weit entfernten Ort gereist.", "{0}{1} [normal]has traveled to a place far away." }},
            
            {"ping", new string[] { "{0}{1} [normal]hat einen Ping von [red]{2}ms[normal].", "{0}{1} [normal]has a ping of [red]{2}ms[normal]." }},

            {"coordinates", new string[] { "Du befindest dich bei [red]X[normal]: {0}, [red]Z[normal]: {2} (und [red]Y[normal]: {1}).", "You are currently at [red]X[normal]: {0}, [red]Z[normal]: {2} (and [red]Y[normal]: {1})." }},

            {"irc", new string[] { "[[red]IRC[normal]] [green]{0}[normal]: [color#FFFFFF]{1}", "" }},

            {"reporthow", new string[] { "Verwendung: [red]/report BiG|Raven hat sich durch meine Wand geglitcht.", "Usage: [red]/report BiG|Raven glitched through my wall." }},
            {"reported", new string[] { "Danke für deinen Report, {0}{1}[normal]. Ein Admin wird sich gleich darum kümmern.", "Thanks for your report, {0}{1}[normal]. An admin will process it in a bit." }},
            {"admhelp", new string[] { "{0}{1}[normal], wenn du einen [red]Admin [normal]benötigst, verwende bitte [red]/report[normal].", "{0}{1}[normal], if you require an [red]admin [normal], please use [red]/report[normal]." }},

            {"groups", new string[] { "Es sind [red]{0} [normal]von [red]{1} [normal]Gruppen aktiv:", "There are [red]{0} [normal]out of [red]{1} [normal]groups active:" }},
            {"group_members", new string[] { "Die Gruppe [red]{0} [normal]hat folgende Mitglieder:", "The group [red]{0} [normal]has the following members:" }},
            {"group_already", new string[] { "Du bist bereits Mitglied in einer Gruppe. Verlasse sie mit [red]/gquit [normal] und versuche es erneut.", "You already are member of a group. Leave it using [red]/gquit [normal]and try again." }},
            {"group_create_how", new string[] { "Verwendung: [red]/gcreate Mein Gruppenname Hier", "Usage: [red]/gcreate My group name here" }},
            {"group_exist", new string[] { "Es existiert bereits eine Gruppe namens [red]{0}[normal].", "A group named [red]{0} [normal]already exists." }},
            {"group_created", new string[] { "{0}{1} [normal]hat die Gruppe [red]{2} [normal]gegründet.", "{0}{1} [normal]has established the group [red]{2}[normal]." }},
            {"group_noadmin", new string[] { "Du musst Gründer einer Gruppe sein um Spieler einzuladen.", "You need to be the founder of a group to invite other players." }},
            {"group_invite_how", new string[] { "Verwendung: [red]/ginvite Spielername [normal](Spielername kann ein Teil oder der gesamte Name sein).", "Usage [red]/ginvite Player name [normal](Player name can be a part or the whole name)." }},
            {"group_invite", new string[] { "{0}{1}[normal], du wurdest von {2}{3} [normal]in die Gruppe [red]{4} [normal]eingeladen. Akzeptiere mit [red]/gaccept[normal].", "{0}{1}[normal], you have been invited by {2}{3}[normal] to join the group [red]{4}[normal]. Accept using [red]/gaccept[normal]." }},
            {"group_already_specific", new string[] { "{0}{1} [normal]ist bereits Mitglied in einer Gruppe.", "{0}{1} [normal]already is a member of a group." }},
            {"group_noinvite", new string[] { "Du hast keine ausstehenden Einladungen.", "You have no pending invites." }},
            {"group_accepted", new string[] { "{0}{1} [normal]ist der Gruppe [red]{2} [normal]beigetreten.", "{0}{1} [normal]has joined the group [red]{2}[normal]." }},
            {"group_kick_how", new string[] { "Verwendung: [red]/gkick Spielername [normal](Spielername kann ein Teil oder der gesamte Name sein).", "Usage [red]/gkick Player name [normal](Player name can be a part or the whole name)." }},
            {"nomember", new string[] { "Kein Spieler mit [red]{0} [normal]im Namen ist Mitglied der Gruppe.", "No player having [red]{0} [normal]in the name is member of the group." }},
            {"gselfkick", new string[] { "Du kannst dich nicht selbst kicken. Verwende [red]/gquit [normal]stattdessen.", "You can't kick yourself. Use [red]/gquit [normal]instead." }},
            {"gkicked", new string[] { "{0}{1} [normal]wurde aus der Gruppe [red]{2} [normal]gekickt.", "{0}{1} [normal]was kicked from the group [red]{2}[normal]." }},
            {"nogroup", new string[] { "Du bist kein Mitglied einer Gruppe.", "You are not member of a group." }},
            {"gquit", new string[] { "{0}{1} [normal]hat die Gruppe [red]{2} [normal]verlassen.", "{0}{1} [normal]has left the group [red]{2}[normal]." }},
            {"group_alliance", new string[] { "[red]{0} [normal]ist Mitglied der Allianz [green]{1}[normal].", "[red]{0} [normal]is member of the alliance [green]{1}[normal]." }},

            {"alliances", new string[] { "Es sind [green]{0} [normal]von [green]{1} [normal]Allianzen aktiv:", "There are [green]{0} [normal]out of [green]{1} [normal]alliances active:" }},
            {"alliance_members", new string[] { "Die Allianz [green]{0} [normal]hat folgende Mitglieder:", "The alliance [green]{0} [normal]has the following members:" }},
            {"alliance_already", new string[] { "Deine Gruppe ist bereits Mitglied in einer Allianz. Verlasse sie mit [red]/aquit [normal] und versuche es erneut.", "Your group already is member of an alliance. Leave it using [red]/aquit [normal]and try again." }},
            {"alliance_create_how", new string[] { "Verwendung: [red]/acreate Mein Allianzname Hier", "Usage: [red]/acreate My alliance name here" }},
            {"alliance_exist", new string[] { "Es existiert bereits eine Allianz namens [green]{0}[normal].", "An alliance named [green]{0} [normal]already exists." }},
            {"alliance_created", new string[] { "{0}{1} [normal]hat die Allianz [green]{2} [normal]gegründet.", "{0}{1} [normal]has established the alliance [green]{2}[normal]." }},
            {"alliance_noadmin", new string[] { "Du musst Gründer einer Allianz sein um das zu tun.", "You need to be the founder of an alliance to do that." }},
            {"alliance_invite_how", new string[] { "Verwendung: [red]/ainvite Gruppenname [normal](Gruppenname kann ein Teil oder der gesamte Name sein).", "Usage [red]/ainvite Group name [normal](Group name can be a part or the whole name)." }},
            {"alliance_already_specific", new string[] { "[red]{0} [normal]ist bereits Mitglied in einer Allianz.", "[green]{0} [normal]already is a member of an alliance." }},
            {"alliance_invite", new string[] { "{0}{1}[normal], deine Gruppe wurde von {2}{3} [normal]in die Allianz [green]{4} [normal]eingeladen. Akzeptiere mit [red]/aaccept[normal].", "{0}{1}[normal], your group has been invited by {2}{3}[normal] to join the alliance [green]{4}[normal]. Accept using [red]/aaccept[normal]." }},
            {"alliance_noinvite", new string[] { "Deine Gruppe hat keine ausstehenden Einladungen.", "Your group has no pending invites." }},
            {"alliance_accepted", new string[] { "[red]{0} [normal]ist der Allianz [green]{1} [normal]beigetreten.", "[green]{0} [normal]has joined the alliance [green]{1}[normal]." }},
            {"alliance_kick_how", new string[] { "Verwendung: [red]/akick Gruppenname [normal](Gruppenname kann ein Teil oder der gesamte Name sein).", "Usage [red]/akick Group name [normal](Group name can be a part or the whole name)." }},
            {"noamember", new string[] { "Keine Gruppe mit [red]{0} [normal]im Namen ist Mitglied der Allianz.", "No group having [green]{0} [normal]in the name is member of the alliance." }},
            {"aselfkick", new string[] { "Du kannst nicht deine eigene Gruppe kicken. Verwende [red]/aquit [normal]stattdessen.", "You can't kick your own group. Use [red]/aquit [normal]instead." }},
            {"akicked", new string[] { "[red]{0} [normal]wurde aus der Allianz [green]{1} [normal]gekickt.", "[green]{0} [normal]was kicked from the alliance [green]{1}[normal]." }},
            {"noalliance", new string[] { "Deine Gruppe ist kein Mitglied einer Allianz.", "Your group is not member of an alliance." }},
            {"aquit", new string[] { "[red]{0} [normal]hat die Allianz [green]{1} [normal]verlassen.", "[green]{0} [normal]has left the alliance [green]{1}[normal]." }},

            {"karma-good", new string[] { "{0}{1} [normal]hat [green]gutes Karma [normal]von [green]{2}[normal].", "{0}{1} [normal]has [green]good karma [normal]of [green]{2}[normal]." }},
            {"karma-bad", new string[] { "{0}{1} [normal]hat [red]schlechtes Karma [normal]von [red]{2}[normal].", "{0}{1} [normal]has [red]bad karma [normal]of [red]{2}[normal]." }},
            {"karma-neutral", new string[] { "{0}{1} [normal]hat [gold]neutrales Karma [normal]von [gold]{2}[normal].", "{0}{1} [normal]has [gold]neutral karma [normal]of [gold]{2}[normal]." }},
            {"karmahow", new string[] { "Verwendung: [red]/good Spielername oder /bad Spielername [normal](Spielername kann ein Teil oder der gesamte Name sein).", "Usage [red]/good Player name or /bad Player name [normal](Player name can be a part or the whole name)." }},
            {"cmdtime_gk", new string[] { "{0}{1} [normal]hat von deiner Gruppe bereits Karma bekommen. Das nächste mal in [red]{2}[normal].", "{0}{1} [normal]has already received Karma from your group. Available again in [red]{2}[normal]." }},
            {"selfkarma", new string[] { "Du kannst dir selbst kein Karma geben.", "You can not give yourself karma." }},
            {"ismember", new string[] { "Du kannst deiner eigenen Gruppe kein Karma geben.", "You can't give karma to your own group." }},

            {"watchwho", new string[] { "Wer soll überwacht werden?", "Who should be watched?" }},
            {"watched", new string[] { "{0}{1} [normal]wird jetzt vom System überwacht. Mach kein Scheiss, klar?", "{0}{1} [normal]is now being watched by the system. Don't do any crap, understood?" }},
            {"nowatch", new string[] { "{0}{1} [normal]wird nicht überwacht.", "{0}{1} [normal]is not being watched." }},
            {"unwatched", new string[] { "{0}{1} [normal]wird nicht mehr vom System überwacht.", "{0}{1} [normal]is not being watched by the system anymore." }}
        };
        }

        private static Dictionary<string, string[]> language = null;

        public static string Say(string key, params object[] parameters)
        {
            return Say(key, Language.GERMAN, parameters);
        }

        public static string Say(string key, User user, params object[] parameters)
        {
            return Say(key, user == null ? Language.GERMAN : user.Language, parameters);
        }

        private static Dictionary<int, Dictionary<int, string>> numbers = new Dictionary<int, Dictionary<int, string>>()
        {
            {Language.GERMAN, new Dictionary<int, string>()
            {
                {0, "kein"},
                {1, "ein"},
                {2, "zwei"},
                {3, "drei"},
                {4, "vier"},
                {5, "fünf"},
                {6, "sechs"},
                {7, "sieben"},
                {8, "acht"},
                {9, "neun"},
                {10, "zehn"},
                {11, "elf"},
                {12, "zwölf"}
            }},
            {Language.ENGLISH, new Dictionary<int, string>()
            {
                {0, "no"},
                {1, "one"},
                {2, "two"},
                {3, "three"},
                {4, "four"},
                {5, "five"},
                {6, "six"},
                {7, "seven"},
                {8, "eight"},
                {9, "nine"},
                {10, "ten"},
                {11, "eleven"},
                {12, "twelve"}
            }}
        };

        public static string Say(string key, int lang, params object[] parameters)
        {
            // Find text
            if (!language.ContainsKey(key))
                return null;

            // Get text by language
            string text = language[key][0];
            if (lang == Language.ENGLISH)
                text = language[key][1];

            // Format parameters
            for (int i = 0; i < parameters.Length; i++)
            {
                int number = 0;
                if (parameters[i] is int || (parameters[i] is string && int.TryParse((string)parameters[i], out number)))
                {
                    if (parameters[i] is int)
                        number = (int)parameters[i];
                    if (numbers[lang].ContainsKey(number))
                        parameters[i] = numbers[lang][number];
                }
                else if (parameters[i] is string)
                {
                    string para = (string)parameters[i];
                    if (Regex.IsMatch(para, "^#......$"))
                        parameters[i] = String.Format("[color{0}]", para);
                    else
                        parameters[i] = Regex.Replace((string)parameters[i], "([^\\\\])\"", "$1\\\"");
                }
                else if (parameters[i] is DateTime || parameters[i] is TimeSpan)
                {
                    TimeSpan p;
                    if (parameters[i] is DateTime)
                        p = (DateTime)parameters[i] - DateTime.Now;
                    else
                        p = (TimeSpan)parameters[i];
                    if (p.TotalHours > 24)
                    {
                        double value = Math.Round(p.TotalHours / 24, 2);
                        parameters[i] = String.Format("{0:0.00} {1}", value, lang == Language.GERMAN ? String.Format("Tag{0}", value != 1 ? "en" : "") : String.Format("day{0}", value != 1 ? "s" : ""));
                    }
                    else if (p.TotalMinutes > 60)
                    {
                        double value = Math.Round(p.TotalMinutes / 60, 2);
                        parameters[i] = String.Format("{0:0.00} {1}", value, lang == Language.GERMAN ? String.Format("Stunde{0}", value != 1 ? "n" : "") : String.Format("hour{0}", value != 1 ? "s" : ""));
                    }
                    else if (p.TotalSeconds > 60)
                    {
                        double value = Math.Round(p.TotalSeconds / 60, 2);
                        parameters[i] = String.Format("{0:0.00} {1}", value, lang == Language.GERMAN ? String.Format("Minute{0}", value != 1 ? "n" : "") : String.Format("minute{0}", value != 1 ? "s" : ""));
                    }
                    else
                    {
                        double value = Math.Round(p.TotalMilliseconds / 1000, 2);
                        parameters[i] = String.Format("{0:0.00} {1}", value, lang == Language.GERMAN ? String.Format("Sekunde{0}", value != 1 ? "n" : "") : String.Format("second{0}", value != 1 ? "s" : ""));
                    }
                }
                else if (parameters[i] is Auth)
                {
                    Auth auth = (Auth)parameters[i];
                    if (lang == Language.GERMAN)
                    {
                        if (auth.Hierarchy == Auth.None.Hierarchy)
                            parameters[i] = "kein VIP";
                        if (auth.Hierarchy == Auth.Bronze.Hierarchy)
                            parameters[i] = "Bronze VIP";
                        else if (auth.Hierarchy == Auth.Silver.Hierarchy)
                            parameters[i] = "Silber VIP";
                        else if (auth.Hierarchy == Auth.Gold.Hierarchy)
                            parameters[i] = "Gold VIP";
                        else if (auth.Hierarchy == Auth.Platinum.Hierarchy)
                            parameters[i] = "Platin VIP";
                        else if (auth.Hierarchy == Auth.Diamond.Hierarchy)
                            parameters[i] = "Diamant VIP";
                        else if (auth.Hierarchy == Auth.SubMod.Hierarchy)
                            parameters[i] = "Sub Moderator";
                        else if (auth.Hierarchy == Auth.Mod.Hierarchy)
                            parameters[i] = "Moderator";
                        else if (auth.Hierarchy == Auth.ChiefAdmin.Hierarchy)
                            parameters[i] = "Chief Admin";
                        else if (auth.Hierarchy == Auth.Admin.Hierarchy)
                            parameters[i] = "Admin";
                    }
                    else
                    {
                        if (auth.Hierarchy == Auth.None.Hierarchy)
                            parameters[i] = "no VIP";
                        else if (auth.Hierarchy == Auth.Bronze.Hierarchy)
                            parameters[i] = "bronze VIP";
                        else if (auth.Hierarchy == Auth.Silver.Hierarchy)
                            parameters[i] = "silver VIP";
                        else if (auth.Hierarchy == Auth.Gold.Hierarchy)
                            parameters[i] = "gold VIP";
                        else if (auth.Hierarchy == Auth.Platinum.Hierarchy)
                            parameters[i] = "platinum VIP";
                        else if (auth.Hierarchy == Auth.Diamond.Hierarchy)
                            parameters[i] = "diamond VIP";
                        else if (auth.Hierarchy == Auth.SubMod.Hierarchy)
                            parameters[i] = "sub moderator";
                        else if (auth.Hierarchy == Auth.Mod.Hierarchy)
                            parameters[i] = "moderator";
                        else if (auth.Hierarchy == Auth.ChiefAdmin.Hierarchy)
                            parameters[i] = "chief admin";
                        else if (auth.Hierarchy == Auth.Admin.Hierarchy)
                            parameters[i] = "admin";

                    }
                }
            }

            // Replace parameters
            text = String.Format(text, parameters);

            // Replace colours
            foreach (string colorname in colours.Keys)
            {
                if (text.Contains(String.Format("[{0}]", colorname)))
                    text = text.Replace(String.Format("[{0}]", colorname), String.Format("[color{0}]", colours[colorname]));
            }

            // Full colour
            if (!text.StartsWith("[color#"))
                text = String.Format("[color{0}]{1}", colours["normal"], text);

            return String.Format("say \"{0}\"", text);
        }

        internal static List<string> List(List<User> list)
        {
            List<string> result = new List<string>();
            string current = "";
            foreach (User user in list)
            {
                if (String.Format("{0}[color#BBBBBB], [color{1}]{2}", current, user.Colour, user.SafeName).Length > MAX_LEN)
                {
                    result.Add(current);
                    current = String.Format("[color{0}]{1}", user.Colour, user.SafeName);
                }
                else
                {
                    if (String.IsNullOrEmpty(current))
                        current = String.Format("[color{0}]{1}", user.Colour, user.SafeName);
                    else
                        current = String.Format("{0}[color#BBBBBB], [color{1}]{2}", current, user.Colour, user.SafeName);
                }
            }
            result.Add(current);
            return result;
        }

        internal static List<string> List(List<Group> list)
        {
            List<string> result = new List<string>();
            string current = "";
            foreach (Group group in list)
            {
                if (String.Format("{0}[color#BBBBBB], {1}", current, group.Name).Length > MAX_LEN)
                {
                    result.Add(current);
                    current = String.Format("[color#BBBBBB]{0}", group.Name);
                }
                else
                {
                    if (String.IsNullOrEmpty(current))
                        current = String.Format("[color#BBBBBB]{0}", group.Name);
                    else
                        current = String.Format("{0}[color#BBBBBB], {1}", current, group.Name);
                }
            }
            result.Add(current);
            return result;
        }

        internal static IEnumerable<string> List(List<Alliance> list)
        {
            List<string> result = new List<string>();
            string current = "";
            foreach (Alliance alliance in list)
            {
                if (String.Format("{0}[color#BBBBBB], {1}", current, alliance.Name).Length > MAX_LEN)
                {
                    result.Add(current);
                    current = String.Format("[color#BBBBBB]{0}", alliance.Name);
                }
                else
                {
                    if (String.IsNullOrEmpty(current))
                        current = String.Format("[color#BBBBBB]{0}", alliance.Name);
                    else
                        current = String.Format("{0}[color#BBBBBB], {1}", current, alliance.Name);
                }
            }
            result.Add(current);
            return result;
        }
    }
}
