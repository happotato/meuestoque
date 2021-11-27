import * as React from "react";
import { CreateOrderDTO, getProducts, Order, Product } from "~/api";
import { Async, AsyncResult } from "~/components/Async";
import { useSelector } from "~/store";

export interface OrderEditorProps {
  defaultOrder?: Order;
  onSubmit?: (order: CreateOrderDTO) => void;
}

export function OrderEditor(props: OrderEditorProps) {
  const user = useSelector((state) => state.user);

  async function onSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();

    const formData = new FormData(e.currentTarget);
    const isIncoming = formData.get("type") == "incoming";

    if (user) {
      const order = {
        price: parseFloat(formData.get("price") as string),
        quantity: parseInt(formData.get("quantity") as string, 10),
        productId: formData.get("product-id") as string,
        ownerId: user.id,
      };

      props.onSubmit?.({
        ...order,
        quantity: isIncoming ? order.quantity : -order.quantity,
      });
    }
  }

  return (
    <form className="container" onSubmit={onSubmit} autoComplete="off">
      <div className="mb-3">
        <label className="form-label">{"Product"}</label>
        <select name="product-id" className="form-select" required>
          <Async func={(signal) => getProducts(signal)}>
            {(result: AsyncResult<Product[], any>) => {
              switch (result.state) {
                case "complete": {
                  return (
                    <React.Fragment>
                      {result.value.map((product) => (
                        <option key={product.id} value={product.id}>
                          {product.name}
                        </option>
                      ))}
                    </React.Fragment>
                  );
                }

                default: {
                  return <></>;
                }
              }
            }}
          </Async>
        </select>
      </div>
      <div className="mb-3 row">
        <div className="col">
          <label className="form-label">{"Price"}</label>
          <div className="input-group">
            <label className="input-group-text text-muted">{"R$"}</label>
            <input
              name="price"
              type="number"
              className="form-control"
              step="0.1"
              min="0"
              defaultValue={props.defaultOrder?.price ?? 1}
              required
            />
          </div>
        </div>
        <div className="col">
          <label className="form-label">{"Quantity"}</label>
          <input
            name="quantity"
            type="number"
            className="form-control"
            defaultValue={props.defaultOrder?.quantity ?? 1}
            min="0"
            disabled={!!props.defaultOrder}
            required
          />
        </div>
      </div>
      <div className="mb-3">
        <div className="form-check">
          <input
            className="form-check-input"
            type="radio"
            name="type"
            value="incoming"
            defaultChecked
          />
          <label className="form-check-label">{"Incoming"}</label>
        </div>
        <div className="form-check">
          <input
            className="form-check-input"
            type="radio"
            name="type"
            value="outgoing"
          />
          <label className="form-check-label">{"Outgoing"}</label>
        </div>
      </div>
      <div className="mb-3">
        <button type="submit" className="btn btn-sm btn-primary">
          {"Create"}
        </button>
      </div>
    </form>
  );
}
