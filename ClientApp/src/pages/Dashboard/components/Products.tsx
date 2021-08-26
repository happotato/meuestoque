import * as React from "react";
import { Link } from "react-router-dom";
import { createProduct, getProducts, Product } from "~/api";
import { ProductComponent } from "./ProductComponent";
import { ProductEditor } from "./ProductEditor";

export function Products() {
  const [products, setProducts] = React.useState<undefined | Product[]>(
    undefined
  );

  React.useEffect(() => {
    const abortController = new AbortController();

    async function loadProducts() {
      let products = await getProducts(abortController.signal);
      setProducts(
        products.sort((a, b) => {
          const timeA = Date.parse(a.createdAt);
          const timeB = Date.parse(b.createdAt);

          return timeB - timeA;
        })
      );
    }

    loadProducts();

    return () => {
      abortController.abort();
    };
  }, []);

  return (
    <div className="row g-0">
      <div className="col p-4">
        <div className="d-flex flex-row align-items-center mb-4">
          {!products && <b className="text-muted">{"Loading..."}</b>}
          {products && (
            <b className="text-muted">{`${products?.length} Product(s)`}</b>
          )}
          <Link
            className="btn btn-sm btn-primary ms-auto d-xl-none"
            to="/dashboard/products/new"
          >
            <i className="fas fa-plus me-2"></i>
            <span>{"New product"}</span>
          </Link>
        </div>
        <div className="row">
          {products && (
            <React.Fragment>
              {products.map((product) => (
                <div key={product.id} className="mb-2">
                  <ProductComponent product={product} />
                </div>
              ))}
            </React.Fragment>
          )}
        </div>
      </div>
      <div className="col-3 d-none d-xl-block border-start vh-100 bg-light p-4 sticky-top overflow-auto">
        <div className="d-flex flex-row align-items-center mb-4">
          <b className="text-muted">{"Create a new Product"}</b>
        </div>
        <div className="row">
          <ProductEditor
            onSubmit={async (product) => {
              const newProduct = await createProduct(product);
              setProducts([newProduct].concat(products ?? []));
            }}
          />
        </div>
      </div>
    </div>
  );
}
