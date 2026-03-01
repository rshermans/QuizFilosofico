using System.ComponentModel.DataAnnotations;

namespace QuizFilosofico.Models.Validacao;



public class SeExisteImagemNoFicheiro : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success; // A validação será feita por outros atributos (Required, por exemplo)
        }

        var filePath = value.ToString();

        if (!File.Exists(filePath))
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }

}
