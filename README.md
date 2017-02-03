# TheThingsNetwork2GoogleSpreadsheet

Links:
https://www.thethingsnetwork.org/
https://developers.google.com/sheets/api/guides/concepts
https://sandervandevelde.wordpress.com/2016/07/09/access-the-things-network-telemetry-using-c-m2mqtt/


This project shows a way to get LoRa-data with MQTT from The Things Network, convert it to an object that represent data from a KELLER LoRa LEO 5 device (http://www.keller-druck.com/home_e/paprod_e/leo5_e.asp) and upload the new object data onto a Google spreadsheet (https://docs.google.com/spreadsheets/d/1zyTU824zYBeQab6t-LuFtKJish2ltotuEQ_e6wAKyUc/edit?usp=sharing)

Thanks to the diagram and Iframe possibility of a google Spreadsheet it is possible to embedd the charts in a HTML file.
The result is a website (http://www.gsmdata.ch/Lora/Leo5TheThingsNetworkTestModule.html) with actual data from a sensor with LoRa compability.

!!
This program worked with staging.thethingsnetwork.org environment which is deprecated and replaced with console.thethingsnetwork.org
The MQTT access must be changed in order to make it work.
!!


This a .NET project. When loading the solution the credential files are missing and you have to reload the packages.
