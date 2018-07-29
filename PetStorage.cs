// Decompiled with JetBrains decompiler
// Type: AucScanner.PetStorage
// Assembly: AucScanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F26D38D8-5498-496B-8AB7-C8B101AF70B8
// Assembly location: C:\ESD\scanner\AucScanner_RU.exe

using AucScanner.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace AucScanner
{
  public class PetStorage
  {
    private string[] defaultPets = new string[95]
    {
      "Личонок Ландро",
      "Разрушитель XXS-002 Ландро",
      "Око легиона",
      "Детеныш крылатого стража",
      "Пурпурный иглобрюх",
      "Призрачный тигренок",
      "Песочный скарабей",
      "Липкий ужас",
      "Пучинная спора",
      "Красный карпик",
      "Синий карпик",
      "Белый карпик",
      "Зеленый карпик",
      "Лазурный дракончик",
      "Маленький Багровый дракончик",
      "Маленький Изумрудный дракончик",
      "Маленький Бронзовый дракончик",
      "Бомблинг черноплавса",
      "Птенец цзы-кунь",
      "Гиацинтовый ара",
      "Капля И'шараджа",
      "Детеныш из гундрака",
      "Липкий ша-ненок",
      "порождение анимуса",
      "пандаренский дух воды",
      "пандаренский дух воздуха",
      "ползучая лапка",
      "ковок",
      "цыпленок-бройлер",
      "рубиновая капля",
      "лун-лун",
      "око новолуния",
      "проклятый кот",
      "карликовый дикорог",
      "жаркий сполох",
      "лисенок",
      "липкий ужас",
      "детеныш лантикоры",
      "Бананчик",
      "Воздушный змей в виде дракона",
      "Детеныш гиппогрифа",
      "Детёныш ночного саблезуба",
      "Клыкаррский воздушный змей",
      "Общительный грелль",
      "Парящий гримуар",
      "Турбоцыплёнок",
      "Эфириальный продавец душ",
      "Волшебный речной рак",
      "Маленький темный дракончик",
      "Дух нефритового пламени",
      "Охотник кривого клыка",
      "Детеныш летучего хамелеона",
      "Колокол кошмаров",
      "Дикий детеныш",
      "Краб-хребтохват",
      "светляк",
      "Прокисший хмелементаль",
      "Тёмный хмелементаль",
      "Пылающий угольный ползун",
      "Юная хваткая лягушка",
      "Абиссал пустомари",
      "Глаз наблюдателя",
      "Глыбик",
      "горная панда",
      "кальмар вольдемар",
      "мерцающий змейчик",
      "Пандаренский дух земли",
      "пандаренский дух огня",
      "призрачная кошка",
      "птенец терродактиля",
      "ворчун",
      "проглот",
      "смердых",
      "снежная панда",
      "солнечная панда",
      "тюлень",
      "Хиджальский огонек",
      "Искра ненависти",
      "Пылающий угольный ползун",
      "Шкипер волн",
      "Фигурка анубисата",
      "Злой и страшный волчок",
      "Хроминий",
      "Гниленыш",
      "Детеныш летучего хамелеона",
      "Живая жидкость",
      "Багровый плеточник",
      "Кролик ярмарки новолуния",
      "малый дух ясеневого листа",
      "гилнеасский ворон",
      "кошка-талисман",
      "хитробот",
      "заводной гном",
      "маленький дренейский защитник",
      "усиленный манадемон"
    };
    public int petVersion = 4;
    private BackgroundWorker worker = new BackgroundWorker();
    public Dictionary<int, Pet> savedPetList = new Dictionary<int, Pet>();
    public BlizzardAPIExplorer explorer;
    public List<Pet> petList;

    public PetStorage(BlizzardAPIExplorer explorer)
    {
      this.explorer = explorer;
    }

    public void addDefaultPets()
    {
      foreach (string defaultPet in this.defaultPets)
      {
        int petId = this.checkPetForId(defaultPet);
        if (petId > 0)
          this.savePet(petId);
      }
      LocalSettings.settings.DefaultPetsVersion = this.petVersion;
      LocalSettings.Save();
    }

    public void refreshData(RefreshCompleteDelegate refreshDelegate)
    {
      this.worker.DoWork += new DoWorkEventHandler(this.Worker_DoWork);
      this.worker.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, e) => refreshDelegate());
      this.worker.RunWorkerAsync();
    }

    private void Worker_DoWork(object sender, DoWorkEventArgs e)
    {
      this.petList = this.explorer.GetPets();
      if (this.petList == null)
      {
        int num = (int) MessageBox.Show("Ошибка!, нет обновления петов");
      }
      else
      {
        foreach (Pet pet in this.petList)
        {
          int speciesId = pet.Stats.SpeciesId;
          if (LocalSettings.settings.StoredPets.ContainsKey((long) speciesId))
            this.savedPetList.Add(pet.Stats.SpeciesId, pet);
        }
      }
    }

    public void savePet(int petId)
    {
      if (!LocalSettings.settings.StoredPets.ContainsKey((long) petId))
      {
        LocalSettings.settings.StoredPets.Add((long) petId, -1L);
        LocalSettings.Save();
      }
      foreach (Pet pet in this.petList)
      {
        if (pet.Stats.SpeciesId == petId)
        {
          this.savedPetList.Add(pet.Stats.SpeciesId, pet);
          break;
        }
      }
    }

    public void deletePet(int petId)
    {
      this.savedPetList.Remove(petId);
      LocalSettings.settings.StoredPets.Remove((long) petId);
      LocalSettings.Save();
    }

    public int checkPetForId(string petName)
    {
      foreach (Pet pet in this.savedPetList.Values)
      {
        if (string.Compare(petName, pet.Name, true) == 0)
          return -1;
      }
      foreach (Pet pet in this.petList)
      {
        if (string.Compare(petName, pet.Name, true) == 0)
          return pet.Stats.SpeciesId;
      }
      return -2;
    }

    public Pet[] savedPets()
    {
      return this.savedPetList.Values.ToArray<Pet>();
    }
  }
}
