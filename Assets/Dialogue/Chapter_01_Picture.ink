#monologue
#speaker:Lavinia

VAR is_first_visit    = true
VAR was_picture_moved = false

=== EntryPoint ===
{was_picture_moved: -> Interact }
{not is_first_visit && not was_picture_moved: -> MovePicture }
{is_first_visit: Не мне судить, но выбор картин максимально странный. }
~ is_first_visit = false
-> MovePicture


== MovePicture
Хм, кажется, за одной из них что−то есть.
    + Впрочем, всё равно. -> END
    + Проверим-ка...
    > SetPosition picture/Sprite Vector3(-5.93,-1.52,0.49)
    ~ was_picture_moved = true
Разумеется, нужен пароль.
-> Interact

== Interact
> ShowUI Strongbox
->END