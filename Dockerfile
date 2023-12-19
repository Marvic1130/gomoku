#FROM openjdk:17-jdk AS build
#WORKDIR /app
#COPY . /app
#RUN microdnf install -y findutils
#RUN chmod +x ./gradlew && ./gradlew bootjar
#ENTRYPOINT ["sh", "-c", "java ${JAVA_OPTS} -jar /app/build/libs/gomoku-0.0.1-SNAPSHOT.jar"]

# 1. Jasypt 를 적용한 프로젝트
# 필요한 종속 항목 설치 및 jar 파일 생성
FROM openjdk:11-jdk AS build
WORKDIR /app
COPY . /app
RUN apt-get update && apt-get install -y findutils
RUN chmod +x ./gradlew && ./gradlew clean bootJar

FROM openjdk:11-jdk
WORKDIR /app
COPY --from=build /app/build/libs/gomoku-0.0.1-SNAPSHOT.jar /app/gomoku.jar
ENTRYPOINT ["sh", "-c", "java ${JAVA_OPTS} -jar /app/gomoku.jar"]






