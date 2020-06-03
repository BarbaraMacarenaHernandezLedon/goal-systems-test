# goal-systems-test
Proyecto que contiene un API REST y un cliente utilizando MaterializeCss desarrollado con el framework ASP.NET MVC.

Este repositorio contiene el código de un inventario al que puedes añadir o eliminar elementos, además de enviar notificaciones a otros procesos con respecto a eventualidades sobre el mismo.

# Instrucciones de ejecución

- Para cambiar la ejecución de la tarea en segundo plano que revisa los elementos caducados:
Inventario.Notification.Hosting.ScheduleItemExpirationVerification

# Diseño
La aplicación web esta desarrollada bajo la arquitectura de software APIREST y cuenta con dos capas que se conectan a través de la misma:
## - Backend-Servidor (Inventary)
 El backend esta desarrollado siguiendo la arquitectura de software APIREST. En este se administra el inventario, y esta compuesto por dos grandes tipos, Items y Notificaciones, ambos contienen su respectivo modelo, la declaracion de la interfaz de sus metodos, y la implementación de los mismos.
 Se implemento el API REST que recibe las llamadas HTTP del cliente, en el cual se definen los endpoints en conjunto con las acciones a desarrollar según la petición y se hace uso de los metodos de cada modelo del inventario.
El servidor cuenta con un servicio de ejecución de tareas en segundo plano utilizando IHostedService.
 Además, este servicio cuenta con Test unitarios que verifican el correcto funcionamiento de todas las llamadas del API.
 
## - Frontend-Cliente (Client)
 El frontend de esta aplicación esta desarrollado siguiendo el patrón de diseño MVC. En este se definen los modelos necesarios para el inventario ("items", "notifications"), las vistas requeridas, y el controlador que realiza las peticiones al servidor.
 
# Anotaciones adicionales
Al ser un proyecto de prueba, fueron muchas las soluciones óptimas que no se realizaron por ser muy costosas de tiempo, estas se describen a continuación:

## - Rendimiento
  Desplegar el backend en un servicio en la nube como AWS o Azure, esto para poder escalar los requerimientos de acuerdo a las necesidades del sistema, hacer uso de cualquier otra funcionalidad en la nube como S3 en AWS o Blob Storage en Azure.

  Una manera de mejorar el rendimiento de la aplicación seria descargar las librerías de MaterializeCss en el servidor de la aplicación, o utilizar algún servidor propio que sirva estos archivos. En este caso, se tomo la decisión de usar el CDN que provee MaterializeCss.

## - Seguridad
  La seguridad es sumamente importante en los proyectos, por lo cual seria ideal implementar las políticas de autorización requeridas para poder acceder al API haciendo uso de la libreria Microsoft.AspNetCore.Authorization, ya que con esta, se pueden definir en el "starup" del servidor todos las condiciones que debe cumplir un usuario para que pueda acceder al mismo.
  
   Además de esto, es indispensable el uso de protocolos criptográficos como SSL para garantizar la seguridad de la comunicación entre procesos y proteger el servicio de CSRF asumiendo que se contará con un modelo de usuarios.
  
  Por otra parte, es importante utilizar el mecanismo de seguridad CORS en donde podamos definir que dominio puede realizar peticiones a nuestro servidor.
  
## - Mantenimiento
  Verificar que el certificado de SSL no se haya vencido.
  
  Mantener las librerías actualizadas, ya que esto podría producir agujeros de seguridad además de un mal funcionamiento de la aplicación.   
  
  A efectos de la prueba no era necesario realizar una base de datos, por lo cual tome la decisión de crear una estructura de datos ConcurrentDictionary<"id",elemento> para almacenar en memoria, esto para poder contar con sus métodos ya definidos, sin embargo, si la aplicación estuviese en producción realizar una base de datos relacional seria ideal para esta aplicación, ya que podrían realizarse búsquedas con respecto a ellas mucho mas ágiles.
      
# Explicación de asunciones

  Con respecto a los requerimiento 1.3 y 1.4, asumí que lo que se quería era que al momento de que un elemento se sacara del inventario o estuviese caducado, el servidor enviase una notificación a otro proceso el cual debería asumir las acciones a realizar debido a lo ocurrido. 
  
  Realice un modelo de notificaciones, el cual cuenta con sus propios métodos de manejo. Este contiene:
  - Id.
  - El tipo de la notificación (removido o caducado).
  - El item removido o caducado.
  - La fecha en la cual se detecto la incidencia.
  
  Y los métodos son agregar una nueva notificación y obtener todas las notificaciones del inventario.
  
  Para el requerimiento 1.3 decidí que una vez verificada la eliminación de un elemento del inventario, se hiciera un llamado al método de agregar una notificación del tipo "remove", sin embargo, a efectos de cumplir explicitamente con el requisito, se debería realizar cuando esto suceda una función que avise a otro proceso que esto ha sucedido. 

Por otra parte, para el requerimiento 1.4. he creado un servicio utilizando la interfaz IHostedService que ejecuta en segundo plano la tarea de verificación de la fecha de caducidad todos los dias a las 3:00 horas. Al ejecutar esta tarea, se verifican en toda la lista de elementos del inventario cuales han caducado, añadiendo una notifiación del tipo "expirate" en el caso de los elementos vencidos, y a su vez eliminandolos del inventario, ya que asumi que al estar caducado no debería permanecer en el mimsmo, y asi de este elemento solo se tendría la notificación de tipo "expirate". Del mismo modo que en el requerimiento anterior, se supone que al agregar una notificación se realice la llamada al proceso que requiere conocer del acontecimiento. 

En ambos casos, considero que, si por ejemplo, la aplicación estuviese en Azure, se podría generar cada vez que una notificación fuese agregada (se llame al metodo "Add" de "notification") un AzureFunction que indique que debe enviar y a que proceso.

Para realizar el servicio ScheduleItemVerification me guie de los procedimientos que siguen en estas dos URLs:
https://thinkrethink.net/2018/05/31/run-scheduled-background-tasks-in-asp-net-core/
https://thinkrethink.net/2018/02/21/asp-net-core-background-processing/
Ya que en anteriores ocaciones no habia tenido la oportunidad de trabajar con IHostedService, esto debido a que utilizabamos Azure Function para este tipo de funcionalidades.