# Cross_Inform
 Frequency analysis of text
## Суть алгоритма
Считать файл, поделив его на строки

Строки разбить на слова, убрав пробелы и символы

Вырезать триплеты из слов

Проверить наличие триплетов в словаре, при отсутствии добавить новый

Отсортировать словарь и выбрать первые 10 значений

Вывести результат
## Ход работы
За тестовый файл был взят текстовый файл на 3,85 мб, 30 384 строки

Был создан алгоритм выполняющий задачу. Время его выполнения в среднем было 700 - 800 ms.

В алгоритм были внесены правки и добавлен поток на обработку строк. Время работы примерно 600-700 ms.

Изменен алгоритм и добавлен поток на построчное чтение из файла. Ожидалось, что пока один считывает другой обрабатывает, но время выполнения стало 850 ms и больше. (лежит в ветке `Slow`)

Изменения были отменены. А алгоритм переделан на поддержку количества потоков, зависящего только от одной переменной. Время работы улучшилось самое быстрое, которое было зафиксировано составляло 548ms. Но данный метод выдавал постоянно ошибки и неточности в расчетах. Связанно это было с массивом словарей, когда несколько потоков работали с каждым из своих словарей, в одном из потоков происходила проверка на ключ, которая показывала, что ключа нет, но когда доходило до добавления программа выдавала ошибку, что ключ уже есть. Плюс при дебагинге было видно, что появляются null значения в словаре.

Был написан алгоритм с ручным распределением потоков, атрибутов и действий на 4 части, с которым удалось достичь наилучшего времени в 472ms (Ветка `FastVersion`)

Поэтому были имплементированы части из каждого способа. В среднем работа программы занимает 600 ms (Ветка `main`)
