# IT100 C# API

como dice el titulo, es una API para conectarse a un IT100 desde C#, tambien incluye integracion con MQTT en caso que se queira usar con un proyecto de IoT
## Instalacion

Clone el repositorio e importe el proyecto desde la IDE de visual studio, ademas agrege las referencias al mismo para poder utilizarlo

## Dependencias

al usar la integracion con MQTT se utiliza [MQTTnet](https://github.com/chkr1011/MQTTnet)


## Uso

en caso se usar solamente la interfaz con el IT100 utilize el siguiente codigo
```csharp
  //inicio de la libreria:
  IT100_API.Base it = new IT100_API.Base();
  it.Init("COM1"); //Puerto al que esta conectado el IT100

```
si no sabes el puerto (y es una aplicacion de consola), solo tenes que iniciar sin el puerto
se mostrara un menu para seleccionar entre los diferentes puertos de la pc
```csharp
  //inicio de la libreria:
  IT100_API.Base it = new IT100_API.Base();
  it.Init();

```

para usar la interfaz MQTT se tiene que cambiar "Base" por "Base_IoT", ademas se tiene que incluir la ruta completa del archivo de configuracion, la estructura del mismo se detalla mas abajo

```csharp
    //inicio de la libreria:
     IT100_API.Base_IoT it = new IT100_API.Base_IoT();
     it.Init("cfg.txt","COM1"); 
```

si se quiere elegir entre varios puertos se usa el mismo menu que el caso anterior sin MQTT

```csharp
    //inicio de la libreria:
     IT100_API.Base_IoT it = new IT100_API.Base_IoT();
     it.Init("cfg.txt");

```
## Comandos
desde la libreria se pueden enviar y recibir comandos del IT100 para esto utilize el siguiente codigo (independientemente si se utiliza el MQTT o no)
, los mismos se encuentran en este link de DSC, [IT100 Manual](http://cms.dsc.com/download.php?t=1&id=16238)
### enviar:

```csharp
  //inicio de la libreria:
   IT100_API.Base it = new IT100_API.Base();
   it.Init("COM1"); //inicio
   it.Send("654","3"); //primer string, el comando, segundo string, la informacion
```
### recibir:
en caso de no tener nuevos mensajes, se obtiene un "-1", luego de procesar los mismos, se necesita borrar la lista para no tener duplicados
```csharp
  //inicio de la libreria:
   IT100_API.Base it = new IT100_API.Base();
   it.Init("COM1"); //inicio

   foreach ( string item in it.Data() ) {
        if ( !item.StartsWith("-1") ) {
             Console.WriteLine("Nuevo mensaje del IT100: {0}" , item);
        }
   }
   it.IT100_Data.Clear(); //borro la lista luego de usarla
```

## Uso de MQTT
al utilizar el MQTT, se pueden enviar y recibir comandos atravez de diferentes canales del mismo (se especifican en el archivo de configuracion) ademas de utilizar la libreria como se mostro anteriormente
. para esto, se tiene que usar el void "Loop()" que sincroniza el IT100 con los canales MQTT que se hayan elegido, los mismos, se separan en canales para comandos de zonas, de particion y luego el resto, aunque se pueden sobreescribir para mandar todo por un unico canal

```csharp
//inicio de la libreria:
IT100_API.Base_IoT it = new IT100_API.Base_IoT();
it.Init("cfg.txt","COM1"); //inicio
for(;;){
    it.Loop();
    //resto del codigo de apliacion
    System.Threading.Thread.Sleep((int) System.TimeSpan.FromSeconds(1.2).TotalMilliseconds);
}
```

## Archivo de configuracion
en el mismo, se especifica la ip o direccion del Broker MQTT , la identidad con el que se va a conectar y los tres canales, (particion, zonas y otros), el mismo tiene la siguiente estructura
```csharp
   cliente,MQTT_BOT1
   ip,192.168.0.12
   Particion,canal A
   Zonas,canal B
   Otros,Canal C

```
## Proximas mejoras:
- agregar el void Loop() a una tarea asincronica
- mejorar el seguimiento de fallos y log
## Licencia
[MIT License](https://choosealicense.com/licenses/mit/)