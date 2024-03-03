# SWF

La idea de SWF nace poder combinar mis habilidades de programación y tecnología con mi memoria y espiritú futbolero, siempre tuve ganas de rendir homenaje a estos jugadores que marcaron mi época (nací en el 2000) y el fin de esto fue desarrollar un bot, que postea por 6 meses (1/3/2024 - 31/8/2024) 184 jugadores que las calles no olvidarán del fútbol argentino.
Para eso, el repositorio contiene mi carpeta Data y tres proyectos diferentes: una API, un cliente y una aplicación de Consola.

## Data

- En este excel armé la estructura de los datos, 31 filas y 6 columnas que comencé a completar de memoria, luego busque todos los equipos destacados de esos años para ayudarme, luego contabilicé por años y equipos para que sea lo más repartido posible.

## API

- Con la API primero poblé la base de datos usando Entity Framework y luego accedo a los datos.

## Cliente

- El cliente es una sencilla aplicación que utilicé para que me sea mas sencillo poblar mi base de datos, reutilizando código arme combos para ir modificado la relacion entre mis tablas Jugadores, Fechas, Campeonatos y Tweets.
  
## Aplicación de Consola

- La aplicación de consola es una herramienta que desarrolle para darle vida al bot que postea haciendo uso de la api v2 de X, use la librería TweetInvi pero al haberse actualizado la api v1.1 de X algunos de sus métodos ya no me eran funcionales asique la request la arme de manera manual asi también como coniguré el programador de tareas de Windows para setear el horario en el que postea el bot porque la api no da acceso a post programados.
