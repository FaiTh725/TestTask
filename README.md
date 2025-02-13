      Гайд для запуска
1) Билдим все необходимые docker image(в тукущей папке)
  1.1) Api с авторизацией и событим
    - cd backend
    - docker build -t test_authorize -f Dockerfile.Authorize .
    - docker build -t test_event -f Dockerfile.Event .
  1.2) Клиенская часть, перейдите в корневую папку(cd ../)
    - cd frontend/Event
    - docker build -t test_front -f Dockerfile.Front .
2) Переходим в папку с бэком и запускаем docker compose(cd ../../ для перехода в корень)
  - cd backend
  - docker compose up

порты:
 - 5101, 5102 - Authorize Api
 - 5201, 5202 - Event Api
 - 8080 - Front(Можно сгенерить сертификат внутри контейнера и будет работать ssl порт 8081)
































Если чет не робит - https://new.online-chasovnya.ru/category/postavit-svechku/besplatno/



