# SWF

La idea de SWF nace para combinar mis habilidades de programación y tecnología con mi memoria y espíritu futbolero. Siempre tuve ganas de rendir homenaje a aquellos jugadores que marcaron mi época (nací en el 2000), y el objetivo de esto fue desarrollar un bot que postee durante 6 meses (1/3/2024 - 31/8/2024) 184 jugadores que las calles no olvidarán del fútbol argentino. Para lograrlo, el repositorio contiene mi carpeta de datos y tres proyectos diferentes: una API, un cliente y una aplicación de consola.

## data

- En este Excel armé la estructura de los datos, con 31 filas y 6 columnas. Comencé a completarlos de memoria y luego busqué todos los equipos destacados de esos años para ayudarme. Luego contabilicé por años y equipos para que sea lo más repartido posible.


## API

- Con la API primero poblé la base de datos usando Entity Framework y luego accedo a los datos.

## Cliente

- El cliente es una sencilla aplicación que utilicé para hacer más dinámico el proceso de poblar mi base de datos. Reutilizando código, armé combos para modificar la relación entre mis tablas Jugadores, Fechas, Campeonatos y Tweets.

## Aplicación de Consola

- La aplicación de consola es una herramienta que desarrollé para darle vida al bot que postea haciendo uso de la API v2 de X. Usé la librería TweetInvi, pero al haberse actualizado la API v1.1 de X, algunos de sus métodos ya no me eran funcionales. Así que armé la request de manera manual, así como configuré el programador de tareas de Windows para establecer el horario en el que el bot postea, porque la API no proporciona acceso a posts programados.
