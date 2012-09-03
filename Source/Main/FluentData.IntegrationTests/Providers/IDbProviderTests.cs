using FluentData._Helpers;

namespace FluentData.Providers
{
	public interface IDbProviderTests
	{
		void Custom_mapper_using_datareader(IDataReader row, Product product);
		void Custom_mapper_using_dynamic(IDataReader row, Product product);
		void Delete_data_sql();
		void Delete_data_builder();
		void In_query();
		void SelectBuilder_Paging();
		void Insert_data_builder_automapping();
		void Insert_data_builder_no_automapping();
		void Insert_data_sql();
		void MultipleResultset();
		void Named_parameters();
		void Query_auto_mapping_alias();
		void Query_custom_mapping_datareader();
		void Query_custom_mapping_dynamic();
		void Query_many_dynamic();
		void Query_many_strongly_typed();
		void Query_single_dynamic();
		void Query_single_strongly_typed();
		void QueryValue();
		void QueryValues();
		void Stored_procedure_builder();
		void Stored_procedure_sql();
		void StoredProcedure_builder_automapping();
		void StoredProcedure_builder_using_expression();
		void Transactions();
		void Unnamed_parameters_many();
		void Unnamed_parameters_one();
		void Update_data_builder();
		void Update_data_builder_automapping();
		void Update_data_sql();
	}
}
