import * as React from "react";
import { Link } from "react-router-dom";
import { createOrder, Product } from "~/api";

export interface ProductComponentProps {
  product: Product;
}

export function ProductComponent(props: ProductComponentProps) {
  const [product, setProduct] = React.useState(props.product);

  async function onIncomeButtonClick(e: React.MouseEvent<HTMLButtonElement>) {
    await createOrder({
      quantity: 1,
      price: product.price,
      productId: product.id,
      ownerId: product.ownerId,
    });

    setProduct({
      ...product,
      quantity: product.quantity + 1,
    });
  }

  async function onOutgoingButtonClick(e: React.MouseEvent<HTMLButtonElement>) {
    await createOrder({
      quantity: -1,
      price: product.price,
      productId: product.id,
      ownerId: product.ownerId,
    });

    setProduct({
      ...product,
      quantity: product.quantity - 1,
    });
  }

  return (
    <div className="card bg-light">
      <div className="card-body lh-sm">
        <div className="row">
          {product.imageUrl && (
            <div className="col-auto">
              <div
                className="bg-light border rounded"
                style={{
                  width: "5rem",
                  height: "5rem",
                }}
              >
                <img
                  className="w-100 h-100 object-fit-contain"
                  src={product.imageUrl}
                />
              </div>
            </div>
          )}
          <div className="col">
            <span>{product.name}</span>
            <br />
            <small className="text-muted">{product.description}</small>
            <br />
            <small className="text-muted">
              <span>{product.quantity}</span>
              <span>{" - "}</span>
              <span>{`R$ ${product.price.toFixed(2).replace(".", ",")}`}</span>
            </small>
          </div>
          <div className="col-auto ms-auto">
            <div className="d-flex flex-row align-items-center">
              <button className="btn btn-sm btn-success me-2" onClick={onIncomeButtonClick}>
                <i className="fas fa-arrow-circle-down me-2"></i>
                <span>{"Incoming"}</span>
              </button>
              <button className="btn btn-sm btn-danger me-2" onClick={onOutgoingButtonClick}>
                <i className="fas fa-arrow-circle-up me-2"></i>
                <span>{"Outgoing"}</span>
              </button>
              <Link
                className="btn btn-sm btn-primary"
                to={`/dashboard/products/${encodeURIComponent(
                  product.id
                )}/edit`}
              >
                <i className="fas fa-edit me-2"></i>
                <span>{"Edit"}</span>
              </Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
