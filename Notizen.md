# Pflichten

## Zielbestimmung

Dezentrales Chatprogramm ist ein Programm, das (Text-)Chats zwischen Personen ermöglicht,
die diese Software auf ihren Computern installiert haben, ohne einen zentralen Server zu
erfordern.

## Musskriterien

* Benutzer können ihre eigene Identität speichern.
* Benutzer können eine Buddyliste führen.
* Benutzer können in einem Fenster chatten.
* Benutzer können den Chat-Verlauf speichern
* Benutzer können sowohl mit einzelnen Benutzern als auch mit einer Gruppe von
  Benutzern chatten.
* Benutzer können andere Benutzer von der Kommunikation mit ihnen ausschließen
  (blockieren).
* Das Programm erlaubt Nachrichten auch dann schreiben, wenn der dedizierte
  Empfänger nicht online ist. Noch nicht ausgelieferte Nachrichten werden
  ausgeliefert, so bald der Empfänger online ist.
* Beim Programmende noch nicht gesendete Nachrichten werden automatisch
  gespeichert.
* Stehen beim Programmstart automatisch gespeicherte Nachrichten an, werden diese
  genauso behandelt wie neue geschriebene.
* Nachrichten haben einen Zeitstempel, aus dem hervorgeht, wann sie geschrieben
  wurden. Dieser wird vom sendenden Client gesetzt. 
* Eine Liste der offenen Chats wird bereitgestellt.

## Sollkriterien

* Die Uhrzeit kann synchronisiert werden.
* Notification ist möglich
* Text-Smileys können in grafische Smileys konvertiert werden
* Die Kommunikation kann verschlüsselt erfolgen.

# Module

## Model


**Repository-Pattern**?

## View

## Controller

# Klassendesign

## Message

|-----------------------
| Message
|-----------------------
|Sender
|Target
|Text
|Time
|-----------------------

## Chat

|-----------------------
| Chat
|-----------------------
|User[] 	Users
|Message[]	Messages
|
|Startzeit?
|-----------------------

## User

|-----------------------
| User
|-----------------------
|IP/Location
|Name
|
|ID?
|-----------------------

## Buddyliste

|-----------------------
|Buddyliste
|-----------------------
|User[]	Users
|
|User Owner?
|-----------------------

# Ideas

Create Controllers and delete them easily.

# Mögliche Fehlerquellen / Probleme

* Nicht synchronisierte Zeit - Nachrichten können dann nicht in der korrekten
  chronologischen Reihenfolge dargestellt werden

* Gruppenchats die nicht mehr aktiv sind, koennen nicht wieder gefunden werden. Bzw. Wie?

|---+----1----+----2----+----3----+----4----+----5----+----6----+----7----+----|