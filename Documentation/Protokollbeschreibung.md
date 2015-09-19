Protokollbeschreibung
=====================

Nachrichten werden als Key-Value-Paare übersendet, die als Strings in folgendem (JSON-ähnlichem) Format vorliegen müssen, bzw. generiert werden:

```{"Key1": "Value1", "Key2": "Value2"}```

Dabei ist die Reihenfolge der Schlüssel nicht relevant. Zwischen den Steuerzeichen (d.h. außerhalb der Anführungszeichen) kann beliebig viel Whitespace enthalten sein (auch Zeilenumbrüche).

Folgende Schlüssel werden erkannt bzw. erzeugt:

|Schlüssel	|Muss	|Beschreibung|
|-----------|-------|-----------|
|```Type```		|*		|	Kann entweder die Werte ```MSG```, ```BINARY``` oder ```CLOSE``` enthalten, wobei zur Zeit lediglich Nachrichten vom Typ ```MSG``` verarbeitet werden.|
|```Content```	|	*	|	Der Inhalt der Nachricht|
|```UserFrom```	|	*	|	Der Benutzername des Senders		|
|```UserTo```		|	*	|	Der Benutzername des Empfängers|
|```IpFrom```		|		|		Die Ip-Addresse des Senders|
|```IpTo```		|		|		Die Ip-Addresse des Empfängers|
|```PortFrom```	|		|		Der Listen-Port des Senders|
|```PortTo```		|		|		Der Listen-Port des Empfängers|


Eine beispielhafte Nachricht könnte folgendermaßen aussehen:

```{ "Type": "MSG", "Content": "Hallo", "UserFrom": "Anna", "UserTo": "Bernd", "IpFrom": "123.123.123.123", "IpTo": "123.123.123.124", "PortFrom": "1234", "PortTo": "1235" }```

Im Programm ist das Erzeugen und Auslesen der Nachrichten ist Aufgabe der Klasse ```NetworkMessageInterpreter```, die eine Reihe von statischen Methoden zu diesem Zweck zur Verfügung stellt.