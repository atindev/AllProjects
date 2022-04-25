using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Validation;
using Microsoft.OData.Edm.Vocabularies;
using System.Collections.Generic;
using System.Xml;

namespace OdataLangTry
{
    public class SampleModelBuilder
    {
        private readonly EdmModel _model = new EdmModel();
        private EdmComplexType _addressType;
        private EdmEnumType _categoryType;
        private EdmEntityType _customerType;
        private EdmEntityContainer _defaultContainer;
        private EdmEntitySet _customerSet;

        public IEdmModel GetModel()
        {
            return _model;
        }

        public void Check()
        {
            var builder = new SampleModelBuilder();
            var model = builder
                .BuildAddressType()
                .BuildCategoryType()
                .BuildCustomerType()
                .BuildDefaultContainer()
                .BuildCustomerSet()
                .BuildAnnotations()
                .GetModel();
            WriteModelToCsdl(model, "csdl.xml");
        }

        private void WriteModelToCsdl(IEdmModel model, string fileName)
        {
            using (var writer = XmlWriter.Create(fileName))
            {
                IEnumerable<EdmError> errors;
                CsdlWriter.TryWriteCsdl(model, writer, CsdlTarget.OData, out errors);
            }
        }

        public SampleModelBuilder BuildAddressType()
        {
            _addressType = new EdmComplexType("Sample.NS", "Address");
            _addressType.AddStructuralProperty("Street", EdmPrimitiveTypeKind.String);
            _addressType.AddStructuralProperty("City", EdmPrimitiveTypeKind.String);
            _addressType.AddStructuralProperty("PostalCode", EdmPrimitiveTypeKind.Int32);
            _model.AddElement(_addressType);
            return this;
        }


        public SampleModelBuilder BuildCategoryType()
        {
            _categoryType = new EdmEnumType("Sample.NS", "Category", EdmPrimitiveTypeKind.Int64, isFlags: true);
            _categoryType.AddMember("Books", new EdmEnumMemberValue(1L));
            _categoryType.AddMember("Dresses", new EdmEnumMemberValue(2L));
            _categoryType.AddMember("Sports", new EdmEnumMemberValue(4L));
            _model.AddElement(_categoryType);
            return this;
        }

        public SampleModelBuilder BuildCustomerType()
        {
            _customerType = new EdmEntityType("Sample.NS", "Customer");
            _customerType.AddKeys(_customerType.AddStructuralProperty("Id", EdmPrimitiveTypeKind.Int32, isNullable: false));
            _customerType.AddStructuralProperty("Name", EdmPrimitiveTypeKind.String, isNullable: false);
            _customerType.AddStructuralProperty("Credits",
                new EdmCollectionTypeReference(new EdmCollectionType(EdmCoreModel.Instance.GetInt64(isNullable: true))));
            _customerType.AddStructuralProperty("Interests", new EdmEnumTypeReference(_categoryType, isNullable: true));
            _customerType.AddStructuralProperty("Address", new EdmComplexTypeReference(_addressType, isNullable: false));
            _model.AddElement(_customerType);
            return this;
        }

        public SampleModelBuilder BuildDefaultContainer()
        {
            _defaultContainer = new EdmEntityContainer("Sample.NS", "DefaultContainer");
            _model.AddElement(_defaultContainer);
            return this;
        }

        public SampleModelBuilder BuildCustomerSet()
        {
            _customerSet = _defaultContainer.AddEntitySet("Customers", _customerType);
            return this;
        }

        public SampleModelBuilder BuildAnnotations()
        {
            var term1 = new EdmTerm("Sample.NS", "MaxCount", EdmCoreModel.Instance.GetInt32(true));
            var annotation1 = new EdmVocabularyAnnotation(_customerSet, term1, new EdmIntegerConstant(10000000L));
            _model.AddVocabularyAnnotation(annotation1);

            var term2 = new EdmTerm("Sample.NS", "KeyName", EdmCoreModel.Instance.GetString(true));
            var annotation2 = new EdmVocabularyAnnotation(_customerType, term2, new EdmStringConstant("Id"));
            annotation2.SetSerializationLocation(_model, EdmVocabularyAnnotationSerializationLocation.Inline);
            _model.AddVocabularyAnnotation(annotation2);

            var term3 = new EdmTerm("Sample.NS", "Width", EdmCoreModel.Instance.GetInt32(true));
            var annotation3 = new EdmVocabularyAnnotation(_customerType.FindProperty("Name"), term3, new EdmIntegerConstant(10L));
            annotation3.SetSerializationLocation(_model, EdmVocabularyAnnotationSerializationLocation.Inline);
            _model.AddVocabularyAnnotation(annotation3);
            return this;
        }
    }
}
