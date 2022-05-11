# Notino.Homework

Popis povodneho kodu:

- promena "input" je deklarovana v try bloku a na riadku 33 s nou chceme pracovat, to vsak nie je mozne a kod sa ani neskompiluje, je preto potreba od riadku 33 vlozit vsetko do try bloku aby sa kod skompiloval
- FileStream, StreamReader, StremWriter su IDisposable objekty, takze ich treba pouzivat bud pomocou klucoveho slova "using" alebo pomocou dlhsieho zapisu try{}catch(){}finally{disposable.Dispose()}
- v catch bloku mne osobne vadi, ze sa vyhadzuje nova vynimka rovnakeho typu a navyse sa tym strati stack trace co v inych pripadoch je velky problem, ak je treba vynimku odchytit a premapovat na inu je vhodne pouzit pretazeny konstruktor a chytenu vynimku pouzit ako parameter innerException
- vseobecne ocdhytavat len Exception nie je "good practice" aj ked v mojom rieseni to pouzijem aj ja pre jednoduchost
- riadok 36 a 37 mozu sposobit ReferenceNullException
