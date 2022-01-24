using ISSProjectFINAL.Models;

namespace ISSProjectFINAL.Repos
{
    class FormRepo
    {
        private readonly Dictionary<long, Form> _forms = new();

        public void Create(Form form)
        {
            if (form is null) return;
            var now = DateTime.Now;
            form.DateCreated = DateOnly.FromDateTime(now);
            form.TimeCreated = TimeOnly.FromDateTime(now);
            _forms[form.Id] = form;
        }

        public Form GetById(long Id)
        {
            return _forms[Id];
        }

        public List<Form> GetAll()
        {
            return _forms.Values.ToList();
        }

        public void Update(Form form)
        {
            var existingForm = GetById(form.Id);
            if (existingForm != null)
            {
                _forms[form.Id] = form;
            }
        }

        public void Delete(long Id)
        {
            _forms.Remove(Id);
        }
    }
}
