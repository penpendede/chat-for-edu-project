# Pflichten

## Zielbestimmung

Dezentrales Chatprogramm ist ein Programm, das (Text-)Chats zwischen Personen erm�glicht,
die diese Software auf ihren Computern installiert haben, ohne einen zentralen Server zu
erfordern.

## Musskriterien

* Benutzer k�nnen ihre eigene Identit�t speichern.
* Benutzer k�nnen eine Buddyliste f�hren.
* Benutzer k�nnen in einem Fenster chatten.
* Benutzer k�nnen den Chat-Verlauf speichern
* Benutzer k�nnen sowohl mit einzelnen Benutzern als auch mit einer Gruppe von
  Benutzern chatten.
* Benutzer k�nnen andere Benutzer von der Kommunikation mit ihnen ausschlie�en
  (blockieren).
* Das Programm erlaubt Nachrichten auch dann schreiben, wenn der dedizierte
  Empf�nger nicht online ist. Noch nicht ausgelieferte Nachrichten werden
  ausgeliefert, so bald der Empf�nger online ist.
* Beim Programmende noch nicht gesendete Nachrichten werden automatisch
  gespeichert.
* Stehen beim Programmstart automatisch gespeicherte Nachrichten an, werden diese
  genauso behandelt wie neue geschriebene.
* Nachrichten haben einen Zeitstempel, aus dem hervorgeht, wann sie geschrieben
  wurden. Dieser wird vom sendenden Client gesetzt. 
* Eine Liste der offenen Chats wird bereitgestellt.

## Sollkriterien

* Die Uhrzeit kann synchronisiert werden.
* Notification ist m�glich
* Text-Smileys k�nnen in grafische Smileys konvertiert werden
* Die Kommunikation kann verschl�sselt erfolgen.

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

# M�gliche Fehlerquellen / Probleme

* Nicht synchronisierte Zeit - Nachrichten k�nnen dann nicht in der korrekten
  chronologischen Reihenfolge dargestellt werden

* Gruppenchats die nicht mehr aktiv sind, koennen nicht wieder gefunden werden. Bzw. Wie?

|---+----1----+----2----+----3----+----4----+----5----+----6----+----7----+----|