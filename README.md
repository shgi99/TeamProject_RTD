<div align="center">
  <h2>🎮 Unity 기획 합반 프로젝트: 랜덤 타워 디펜스 🎮</h2>
</div>

<div align="center">
  <img width="300" height="300" alt="GameIcon" src="https://github.com/user-attachments/assets/f52368fd-6b28-49c5-8e93-19cdcef0cad4" />
</div>

# 목차
1. [프로젝트 개요](#프로젝트-개요)
2. [기술 스택](#기술-스택)
3. [구현 요소](#구현-요소)
4. [구현 상세](#구현-상세)
5. [트러블 슈팅](#트러블-슈팅)
6. [후기](#후기)


## 프로젝트 개요
- **Unity**을 활용한 스타크래프트 유즈맵 **랜덤 타워 디펜스** 게임을 모작
- 개발 1명, 기획 1명
- 처음으로 진행한 기획자와의 협업 프로젝트

## 기술 스택

<p align="left">
  <img src="https://img.shields.io/badge/Unity-FFFFFF?style=flat-square&logo=unity&logoColor=black"/>
  <img src="https://img.shields.io/badge/C%23-80247B?style=flat-square&logo=csharp&logoColor=white"/>
  <img src="https://img.shields.io/badge/Visual_Studio-5C2D91?style=flat-square&logo=visual%20studio&logoColor=white"/>
  <img src="https://img.shields.io/badge/GitHub-181717?style=flat-square&logo=github&logoColor=white"/>
  <img src="https://img.shields.io/badge/SourceTree-0052CC?style=flat-square&logo=sourcetree&logoColor=white"/>
</p>

## 주요 구현 요소

| <img width="200" src="https://github.com/user-attachments/assets/1989c40a-59c4-48a4-8a5d-84809066c6f8"/> | <img width="200" src="https://github.com/user-attachments/assets/e8f0486a-edc3-4313-aac2-680e0b3efd9d"/> | <img width="200" src="https://github.com/user-attachments/assets/8b6dc5bd-ed57-443b-b01f-966f14e66d1d"/> | <img width="200" src="https://github.com/user-attachments/assets/6085de4e-af3c-483f-aa07-e17f472f5337"/> | <img width="200" src="https://github.com/user-attachments/assets/11d052ab-de61-4464-b685-b737ab92bd9a"/> |
|:--:|:--:|:--:|:--:|:--:|
| **타워 배치** | **타워 업그레이드** | **타워 합성 및 판매** | **비동기 로딩** | **수집형 퀘스트** |


## 구현 상세
1. ### 타워 배치
  - 타워 설치 버튼을 눌러 타워 배치 모드 전환
  - 설치 가능한 타일을 선택하여 터치
  - 가장 낮은 등급의 타워중 무작위로 선택
  - 미네랄을 소모하여 타워 배치

2. ### 타워 업그레이드
  - 가스를 소모하여 타워 업그레이드
  - 3종류의 종족 구분(인간, 괴물, 기계)하여 개별 업그레이드

3. ### 타워 합성 및 판매
  - 타워 판매 시 미네랄 지급
  - 같은 종류의 타워 2개 합성 가능
  - 합성 진행 시 같은 종류의 타워 중 가장 가까운 타워와 합성 진행
  - 상위 등급의 타워 중 무작위로 하나 생성

4. ### 비동기 로딩
  - 인게임 성능 최적화를 위해 리소스들을 사전에 로딩 씬을 이용해 로딩
  - 이후 오브젝트 풀링을 활용하여 리소스를 재사용하도록 구현

5. ### 수집형 퀘스트
  - 타워 설치 시 타워 수집형 퀘스트 클리어를 체크
  - 클리어 시 인게임 재화 제공

## 트러블 슈팅

| 문제 | 해결 방법 |
|------|------------|
| 스킬 효과 중복 적용 버그 | 코루틴을 이용한 방식에서 시간을 비교하는 방식으로 변경 |

## 후기
- 기획자와 처음으로 협업을 경험해보니 제대로 무언가를 하는거 같아 즐거웠고, 각자 해야할 역할에 대한 책임감에 대해 생각해보는 계기가 되었다.
