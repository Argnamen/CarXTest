## Тестовое задание Unity разработчика в CarX

**ПРЕДСТАВИМ, ЧТО ВЫ СТАНОВИТЕСЬ НОВЫМ И ЕДИНСТВЕННЫМ РАЗРАБОТЧИКОМ ТЕСТОВОГО ПРОЕКТА CARX TOWER DEFENCE. ЗДЕСЬ ПРЕДСТАВЛЕНО ТЕКУЩЕЕ СОСТОЯНИЕ ПРОЕКТА, ВАШИ ЗАДАЧИ В ПОРЯДКЕ ПРИОРИТЕТА:**

##### Обязательное:
1. Сделать стрельбу с упреждением для CannonTower: добавить функционал поиска цели и наведения на неё так, чтобы выпущенные снаряды стали попадать по цели; снаряды должны вылетать строго из ствола пушки; на снаряд в полёте запрещено прикладывать какие-либо силы или корректировки;
2. Для CannonTower сделать захват и сопровождение цели с упреждением; поворот орудия башни должен быть плавным; сделать возможность настройки скорости поворота орудия;
3. Для CannonTower сделать дополнительный режим стрельбы: на упреждение по параболической траектории (на снаряд должна действовать гравитация); должна быть возможность переключать CannonTower между двумя режимами стрельбы без изменения кода;

##### Необязательное:
4. Провести осмотр проекта: перепроектировать, оптимизировать, исправить найденные недочёты, либо изложить их текстом и указать возможные пути решения.
5. Для логики перемещения монстров сделать дополнительный режим движения: с постоянным ускорением; должна быть возможность настройки значения ускорения. Соответственно, при этом башни должны попадать по монстрам-целям. Должна быть возможность переключить режим движения для монстров на любой из всех реализованных режимов движения.
6. Для логики перемещения монстров сделать дополнительный режим движения: по окружности вокруг башен (с постоянной скоростью, по постоянному радиусу; должна быть возможность настройки значений скорости и радиуса). Соответственно, при этом башни должны попадать по монстрам-целям. Должна быть возможность переключить режим движения для монстров на любой из всех реализованных режимов движения.
7. Для логики перемещения монстров сделать дополнительный режим движения: по плавной траектории на основе настраиваемого массива точек в пространстве (чекпоинты), с постоянной скоростью (должна быть возможность настройки значения постоянной скорости). Соответственно, при этом башни должны попадать по монстрам-целям. Должна быть возможность переключить режим движения для монстров на любой из всех реализованных режимов движения.
8. В проекте есть префаб “FlyingShield“. Этот щит кружится вокруг своего владельца (вокруг родительского объекта) с постоянной скоростью, постоянным радиусом и по постоянной оси; все эти параметры являются настраиваемыми и должны остаться таковыми. При этом щит уничтожает любые снаряды, с которыми он сталкивается. Задача состоит в том, чтобы подключить этот летающий щит к монстрам так, чтобы башни стали выстреливать свои снаряды, помимо прочих условий, только в подходящие моменты - когда башня точно “знает”, что её снаряд достигнет монстра, не столкнувшись со щитом.

------------

## Unity developer test Job in CarX

**IMAGINE THAT YOU ARE BECOMING THE NEW AND ONLY DEVELOPER OF THE TEST PROJECT "CARX TOWER DEFENSE". HERE IS PROJECT IN ITS CURRENT STATE, YOUR TASKS IN ORDER OF PRIORITY:**

##### Required:
1. Implement a pre-emptive firing for Cannon Tower: add target search functionality so that the launched projectiles hit the target; the shells must fly straight out of the barrel; on a projectile in flight it is forbidden to apply any forces or corrections;
2. Implement seeking and tracking of target for Cannon Tower; the rotation of the Canon Tower's barrel must be smooth; make it possible to adjust the speed of rotation of the gun;
3. Implement an additional firing mode for the Cannon Tower: preemptive along a parabolic trajectory (gravity must act on the projectile); it should be possible to switch the CannonTower between two firing modes without changing the code;

##### Optional:
4. Inspect the project: refactor, redesign, optimize, correct founded defects. Implement a clear, understandable, simple architecture, without unnecessary clutter, in accordance with the principles of SOLID;
5. For the logic of moving monsters, make an additional movement mode: with constant acceleration; it should be possible to adjust the acceleration value. Accordingly, the towers must hit the target monsters. It should be possible to switch the movement mode for monsters to any of all implemented movement modes.
6. For the logic of moving monsters, make an additional movement mode: in a circle around the towers (at a constant speed, along a constant radius; it should be possible to adjust the values of speed and radius). Accordingly, the towers must hit the target monsters. It should be possible to switch the movement mode for monsters to any of all implemented movement modes.
7. For the logic of moving monsters, make an additional movement mode: along a smooth trajectory based on a configurable array of points in space (checkpoints), at a constant speed (it should be possible to adjust the value of constant speed). Accordingly, the towers must hit the target monsters. It should be possible to switch the movement mode for monsters to any of all implemented movement modes.
8. The project has the prefab “FlyingShield". This shield rotates around its owner (around the parent object) at a constant speed, constant radius and on a constant axis; all these parameters are configurable and must remain so. In doing so, the shield destroys any projectiles it encounters. The task is to connect this flying shield to the monsters so that the towers begin to fire their projectiles, among other conditions, only at the appropriate moments - when the tower “knows” for sure that its projectile will reach the monster without colliding with the shield.