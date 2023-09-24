#monologue
#speaker:Lavinia

VAR is_hatch_opened = false

=== EntryPoint ===
Всегда забываю, что здесь есть люк в подсобку.
Надеюсь, он закрыт, не хотелось бы ночью провалиться в подпол.
    + Проверю.
        {not is_hatch_opened:
            Закрыт, разумеется.
            …
            ++ Открыть…
                ~ is_hatch_opened = true
                > CompletePlotPoint HatchIsOpened
                > CompletePlotPoint HatchIsFound
                -> END
            ++ Вот и славно. -> END
        }
        {is_hatch_opened:
            Открыт…
            Безопасность в этом доме на высоте, как и всегда.
            ++ Но так и задумано. -> END
            ++ Лучше закрою.
                ~ is_hatch_opened = false
                > RemoveAchievedPlotPoint HatchIsOpened
                -> END
        }
    + Глупость, конечно, никуда я не упаду. -> END